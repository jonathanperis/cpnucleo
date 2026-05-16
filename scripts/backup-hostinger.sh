#!/usr/bin/env bash
set -Eeuo pipefail

# Hostinger backup helper for Cpnucleo.
# Run from the directory that contains compose.prod.yaml and the production .env:
#   ./scripts/backup-hostinger.sh
# Optional cron example:
#   15 3 * * * cd /docker/cpnucleo && ./scripts/backup-hostinger.sh >> /var/log/cpnucleo-backup.log 2>&1

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
  esac
done < <(python3 - "${ENV_FILE}" <<'PY'
import ast
import sys

wanted = {"POSTGRES_USER", "POSTGRES_DB", "BACKUP_DIR", "BACKUP_RETENTION_DAYS"}
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

mkdir -p "${DEST}"
chmod 700 "${BACKUP_DIR}" "${DEST}"

# Database logical dump. Custom format keeps restore flexible:
#   docker compose -f compose.prod.yaml exec -T db pg_restore -U "$POSTGRES_USER" -d "$POSTGRES_DB" --clean --if-exists < dump.pgcustom
if docker compose -f "${COMPOSE_FILE}" ps --status running db >/dev/null 2>&1; then
  docker compose -f "${COMPOSE_FILE}" exec -T db \
    pg_dump -U "${POSTGRES_USER}" -d "${POSTGRES_DB}" --format=custom --no-owner --no-acl \
    > "${DEST}/cpnucleo-db.pgcustom"
else
  echo "db service is not running; cannot create pg_dump." >&2
  exit 1
fi

# Deployment/config backup. This intentionally includes .env because it is required
# for disaster recovery; store BACKUP_DIR with restrictive permissions and copy it
# only to a trusted/private off-server location.
tar -czf "${DEST}/cpnucleo-config.tar.gz" \
  "${COMPOSE_FILE}" \
  "${ENV_FILE}" \
  nginx.conf \
  docker-entrypoint-initdb.d \
  2>/dev/null

sha256sum "${DEST}/cpnucleo-db.pgcustom" "${DEST}/cpnucleo-config.tar.gz" \
  > "${DEST}/SHA256SUMS"

# BACKUP_DIR is validated above before any deletion happens.
find "${BACKUP_DIR}" -mindepth 1 -maxdepth 1 -type d -mtime +"${BACKUP_RETENTION_DAYS}" -exec rm -rf {} +

printf 'Cpnucleo backup completed: %s\n' "${DEST}"
