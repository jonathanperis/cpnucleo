#!/usr/bin/env bash
set -Eeuo pipefail

# Proves that a backup written by scripts/backup-hostinger.sh can be restored.
#
# Usage:
#   ./scripts/verify-backup.sh                 # latest complete backup in BACKUP_DIR
#   ./scripts/verify-backup.sh /path/to/backup # one specific backup directory
#
# The restore target is always a disposable postgres:16.15 container that this
# script starts itself, with no network (--network none) and a random password,
# and removes on exit. The script accepts no host, port or connection string, so
# it cannot be pointed at the production database or any other server.
#
# Checks: SHA256SUMS, config archive readability, a full pg_restore with
# --exit-on-error, __EFMigrationsHistory contents and row counts of key tables.

BACKUP_DIR=${BACKUP_DIR:-/opt/backups/cpnucleo}
VERIFY_IMAGE="postgres:16.15"
LABEL="io.cpnucleo.restore-verify=true"

if [[ $# -gt 1 ]]; then
  echo "Usage: $0 [backup-directory]" >&2
  exit 2
fi

if [[ $# -eq 1 ]]; then
  backup="$1"
else
  # Newest timestamped directory that finished (SHA256SUMS is written last).
  backup=""
  while IFS= read -r candidate; do
    if [[ -f "${candidate}/SHA256SUMS" ]]; then
      backup="${candidate}"
    fi
  done < <(find "${BACKUP_DIR}" -mindepth 1 -maxdepth 1 -type d -name '[0-9]*T[0-9]*Z' | sort)
  if [[ -z "${backup}" ]]; then
    echo "No complete backup (directory with SHA256SUMS) found in ${BACKUP_DIR}." >&2
    exit 1
  fi
fi

dump="${backup}/cpnucleo-db.pgcustom"
for required in "${backup}/SHA256SUMS" "${dump}" "${backup}/cpnucleo-config.tar.gz"; do
  if [[ ! -f "${required}" ]]; then
    echo "Missing ${required}; not a complete Cpnucleo backup." >&2
    exit 1
  fi
done

echo "Verifying backup ${backup}"
(cd "${backup}" && sha256sum -c SHA256SUMS)
tar -tzf "${backup}/cpnucleo-config.tar.gz" > /dev/null
echo "Config archive is readable."

container=""
cleanup() {
  if [[ -n "${container}" ]]; then
    docker rm -f "${container}" > /dev/null
    echo "Removed disposable restore container."
  fi
}
trap cleanup EXIT

password=$(openssl rand -hex 24)
container=$(docker run -d --rm \
  --name "cpnucleo-restore-verify-$(openssl rand -hex 4)" \
  --label "${LABEL}" \
  --network none \
  -e POSTGRES_USER=restore_verify \
  -e POSTGRES_PASSWORD="${password}" \
  -e POSTGRES_DB=restore_verify \
  "${VERIFY_IMAGE}")

# Every command below runs only inside the container created above.
in_container() {
  if [[ "$(docker inspect --format '{{index .Config.Labels "io.cpnucleo.restore-verify"}}' "${container}")" != "true" ]]; then
    echo "Refusing to run against a container this script did not create." >&2
    exit 1
  fi
  docker exec -i "${container}" "$@"
}

psql_value() {
  in_container psql -v ON_ERROR_STOP=1 -U restore_verify -d restore_verify -Atqc "$1"
}

# The image's init phase runs a socket-only temporary server; the final server
# is the one that also listens on TCP inside the container.
ready=false
for _ in {1..60}; do
  if in_container pg_isready -q -h 127.0.0.1 -U restore_verify -d restore_verify; then
    ready=true
    break
  fi
  sleep 1
done
if [[ "${ready}" != true ]]; then
  echo "Disposable PostgreSQL did not become ready." >&2
  docker logs "${container}" >&2
  exit 1
fi

in_container pg_restore -U restore_verify -d restore_verify --no-owner --no-acl --exit-on-error < "${dump}"
echo "pg_restore completed without errors."

migrations=$(psql_value 'SELECT count(*) FROM "__EFMigrationsHistory";')
latest=$(psql_value 'SELECT "MigrationId" FROM "__EFMigrationsHistory" ORDER BY "MigrationId" DESC LIMIT 1;')
if [[ "${migrations}" -lt 1 ]]; then
  echo "Restored database has an empty __EFMigrationsHistory." >&2
  exit 1
fi
echo "__EFMigrationsHistory: ${migrations} migration(s), latest ${latest}"

echo "Row counts:"
for table in Organizations Projects Users Assignments Workflows Impediments; do
  exists=$(psql_value "SELECT to_regclass('public.\"${table}\"') IS NOT NULL;")
  if [[ "${exists}" == t ]]; then
    printf '  %-14s %s\n' "${table}" "$(psql_value "SELECT count(*) FROM \"${table}\";")"
  else
    printf '  %-14s %s\n' "${table}" "(table not present)"
  fi
done

echo "Backup ${backup} restored successfully into a disposable container."
