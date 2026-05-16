#!/usr/bin/env bash
set -Eeuo pipefail

# Hostinger backup helper for Cpnucleo.
# Run from the directory that contains compose.prod.yaml and the production .env:
#   ./scripts/backup-hostinger.sh
# Optional cron example:
#   15 3 * * * cd /docker/cpnucleo && ./scripts/backup-hostinger.sh >> /var/log/cpnucleo-backup.log 2>&1

COMPOSE_FILE=${COMPOSE_FILE:-compose.prod.yaml}
ENV_FILE=${ENV_FILE:-.env}
BACKUP_DIR=${BACKUP_DIR:-/opt/backups/cpnucleo}
BACKUP_RETENTION_DAYS=${BACKUP_RETENTION_DAYS:-14}
TIMESTAMP=$(date -u +%Y%m%dT%H%M%SZ)
DEST="${BACKUP_DIR}/${TIMESTAMP}"

if [[ ! -f "${COMPOSE_FILE}" ]]; then
  echo "Missing ${COMPOSE_FILE}; run this script from the Cpnucleo deploy directory." >&2
  exit 1
fi

if [[ ! -f "${ENV_FILE}" ]]; then
  echo "Missing ${ENV_FILE}; production secrets and DB settings are required for backup." >&2
  exit 1
fi

mkdir -p "${DEST}"
chmod 700 "${BACKUP_DIR}" "${DEST}"

# Load POSTGRES_USER/POSTGRES_DB/BACKUP_RETENTION_DAYS/BACKUP_DIR if present.
set -a
# shellcheck disable=SC1090
source "${ENV_FILE}"
set +a

: "${POSTGRES_USER:?POSTGRES_USER must be set in ${ENV_FILE}}"
: "${POSTGRES_DB:?POSTGRES_DB must be set in ${ENV_FILE}}"

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

find "${BACKUP_DIR}" -mindepth 1 -maxdepth 1 -type d -mtime +"${BACKUP_RETENTION_DAYS}" -exec rm -rf {} +

printf 'Cpnucleo backup completed: %s\n' "${DEST}"
