#!/usr/bin/env bash
set -Eeuo pipefail

# Hostinger backup helper for Cpnucleo.
# Run from the directory that contains the production compose file and .env:
#   ./scripts/backup-hostinger.sh
# Hostinger Docker Manager keeps the project as /docker/<project>/docker-compose.yaml:
#   COMPOSE_FILE=docker-compose.yaml COMPOSE_PROJECT_NAME=<project> ./backup-hostinger.sh
# Optional cron example:
#   15 3 * * * cd /docker/cpnucleo && ./scripts/backup-hostinger.sh >> /var/log/cpnucleo-backup.log 2>&1
#
# Steps: pg_dump (custom format) -> pg_restore --list verification -> config
# archive of the files that exist -> SHA256SUMS (written last, so its presence
# marks a complete backup) -> optional off-host copy (BACKUP_REMOTE) -> local
# retention. Retention runs after every verified backup (even if the off-host
# copy fails or is skipped) but never after a failed dump; any failure exits non-zero.
# Prove restorability periodically with scripts/verify-backup.sh.

COMPOSE_FILE=${COMPOSE_FILE:-compose.prod.yaml}
ENV_FILE=${ENV_FILE:-.env}
TIMESTAMP=$(date -u +%Y%m%dT%H%M%SZ)

if [[ ! -f "${COMPOSE_FILE}" ]]; then
  echo "Missing ${COMPOSE_FILE}; run this script from the Cpnucleo deploy directory." >&2
  exit 1
fi

if [[ ! -f "${ENV_FILE}" ]]; then
  echo "Missing ${ENV_FILE}; production secrets and DB settings are required for backup." >&2
  exit 1
fi

# Parse only the keys this script needs from dotenv syntax without executing it.
# This keeps DB_CONNECTION_STRING values with spaces (for example
# "Maximum Pool Size") from breaking the backup script.
while IFS=$'\t' read -r key value; do
  case "${key}" in
    POSTGRES_USER) DOTENV_POSTGRES_USER=${value} ;;
    POSTGRES_DB) DOTENV_POSTGRES_DB=${value} ;;
    BACKUP_DIR) DOTENV_BACKUP_DIR=${value} ;;
    BACKUP_RETENTION_DAYS) DOTENV_BACKUP_RETENTION_DAYS=${value} ;;
    BACKUP_REMOTE) DOTENV_BACKUP_REMOTE=${value} ;;
  esac
done < <(python3 - "${ENV_FILE}" <<'PY'
import ast
import sys

wanted = {"POSTGRES_USER", "POSTGRES_DB", "BACKUP_DIR", "BACKUP_RETENTION_DAYS", "BACKUP_REMOTE"}
path = sys.argv[1]

with open(path, encoding="utf-8") as f:
    for raw in f:
        line = raw.strip()
        if not line or line.startswith("#"):
            continue
        if line.startswith("export "):
            line = line[len("export "):].lstrip()
        if "=" not in line:
            continue
        key, value = line.split("=", 1)
        key = key.strip()
        if key not in wanted:
            continue
        value = value.strip()
        if len(value) >= 2 and value[0] == value[-1] and value[0] in {"'", '"'}:
            try:
                value = ast.literal_eval(value)
            except Exception:
                value = value[1:-1]
        print(f"{key}\t{value}")
PY
)

POSTGRES_USER=${POSTGRES_USER:-${DOTENV_POSTGRES_USER:-}}
POSTGRES_DB=${POSTGRES_DB:-${DOTENV_POSTGRES_DB:-}}
BACKUP_DIR=${BACKUP_DIR:-${DOTENV_BACKUP_DIR:-/opt/backups/cpnucleo}}
BACKUP_RETENTION_DAYS=${BACKUP_RETENTION_DAYS:-${DOTENV_BACKUP_RETENTION_DAYS:-14}}
BACKUP_REMOTE=${BACKUP_REMOTE:-${DOTENV_BACKUP_REMOTE:-}}
DEST="${BACKUP_DIR}/${TIMESTAMP}"

: "${POSTGRES_USER:?POSTGRES_USER must be set in ${ENV_FILE}}"
: "${POSTGRES_DB:?POSTGRES_DB must be set in ${ENV_FILE}}"

case "${BACKUP_DIR}" in
  ""|"/"|"."|"..")
    echo "Refusing unsafe BACKUP_DIR='${BACKUP_DIR}'" >&2
    exit 1
    ;;
  /*) ;;
  *)
    echo "Refusing BACKUP_DIR='${BACKUP_DIR}'; expected an absolute path." >&2
    exit 1
    ;;
esac

if [[ ! "${BACKUP_RETENTION_DAYS}" =~ ^[0-9]+$ ]]; then
  echo "BACKUP_RETENTION_DAYS must be a non-negative integer." >&2
  exit 1
fi

compose=(docker compose --env-file "${ENV_FILE}" -f "${COMPOSE_FILE}")
status=0

# Local retention runs after every verified backup, even when the optional
# off-host copy fails or is skipped. It deliberately does not run after a failed
# dump: repeated failures must never age out the last good backups. Only
# timestamped backup directories are candidates; BACKUP_DIR was validated above.
apply_retention() {
  if [[ -d "${BACKUP_DIR}" ]]; then
    find "${BACKUP_DIR}" -mindepth 1 -maxdepth 1 -type d -name '[0-9]*T[0-9]*Z' \
      -mtime +"${BACKUP_RETENTION_DAYS}" -exec rm -rf {} +
  fi
}

# shellcheck disable=SC2329 # invoked by the ERR trap below
on_error() {
  echo "Backup step failed (line $1); removing incomplete ${DEST}. Older backups are kept." >&2
  rm -rf "${DEST}"
}
trap 'on_error $LINENO' ERR

# `compose ps --status running` exits 0 even when nothing matches, so require
# an actual running container id for the db service.
db_container=$("${compose[@]}" ps --status running --quiet db)
if [[ -z "${db_container}" ]]; then
  echo "No running db container for ${COMPOSE_FILE} (project ${COMPOSE_PROJECT_NAME:-from compose file}); cannot create pg_dump." >&2
  echo "Set COMPOSE_PROJECT_NAME/COMPOSE_FILE to match the deployed project." >&2
  exit 1
fi

mkdir -p "${DEST}"
chmod 700 "${BACKUP_DIR}" "${DEST}"

# Database logical dump. Custom format keeps restore flexible:
#   docker compose -f compose.prod.yaml exec -T db pg_restore -U "$POSTGRES_USER" -d "$POSTGRES_DB" --clean --if-exists < cpnucleo-db.pgcustom
"${compose[@]}" exec -T db \
  pg_dump -U "${POSTGRES_USER}" -d "${POSTGRES_DB}" --format=custom --no-owner --no-acl \
  > "${DEST}/cpnucleo-db.pgcustom"

# The dump must be a readable custom-format archive that contains the EF
# migration history before it is checksummed as a good backup.
"${compose[@]}" exec -T db pg_restore --list < "${DEST}/cpnucleo-db.pgcustom" > "${DEST}/cpnucleo-db.toc"
if ! grep -q '__EFMigrationsHistory' "${DEST}/cpnucleo-db.toc"; then
  echo "Dump verification failed: __EFMigrationsHistory is missing from the archive TOC." >&2
  false
fi
table_data_entries=$(grep -c ' TABLE DATA ' "${DEST}/cpnucleo-db.toc" || true)
echo "Dump verified: ${table_data_entries} table data entries."

# Deployment/config backup. This intentionally includes .env because it is required
# for disaster recovery; store BACKUP_DIR with restrictive permissions and copy it
# only to a trusted/private off-server location. Docker Manager projects inline
# nginx/collector/init configs into the compose file, so optional files are
# archived only when present.
config_files=("${COMPOSE_FILE}" "${ENV_FILE}")
for optional in nginx.conf otel-collector.yaml docker-entrypoint-initdb.d; do
  if [[ -e "${optional}" ]]; then
    config_files+=("${optional}")
  fi
done
tar -czf "${DEST}/cpnucleo-config.tar.gz" "${config_files[@]}"

(cd "${DEST}" && sha256sum cpnucleo-db.pgcustom cpnucleo-db.toc cpnucleo-config.tar.gz > SHA256SUMS)
trap - ERR
printf 'Cpnucleo backup written and verified: %s\n' "${DEST}"

# Optional off-host copy (rsync over SSH), e.g. BACKUP_REMOTE=backup@host:/srv/cpnucleo.
# A failure is reported and fails the run, but retention still happens.
if [[ -n "${BACKUP_REMOTE}" ]]; then
  if command -v rsync >/dev/null; then
    if rsync -a --chmod=D700,F600 -e "ssh ${BACKUP_SSH_OPTIONS:-}" "${DEST}" "${BACKUP_REMOTE%/}/"; then
      echo "Off-host copy completed: ${BACKUP_REMOTE%/}/${TIMESTAMP}"
    else
      echo "Off-host copy to ${BACKUP_REMOTE} failed." >&2
      status=1
    fi
  else
    echo "BACKUP_REMOTE is set but rsync is not installed; off-host copy skipped." >&2
    status=1
  fi
else
  echo "BACKUP_REMOTE not set; keeping the backup on this host only."
fi

apply_retention
exit "${status}"
