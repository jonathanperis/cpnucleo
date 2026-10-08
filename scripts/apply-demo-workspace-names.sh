#!/usr/bin/env bash
set -Eeuo pipefail

# Renames the Bogus-generated demo data of an existing Cpnucleo database to project-workspace
# names (src/Infrastructure/Common/Helpers/DemoWorkspaceNames.sql). Only rows that still carry
# generated text change; people's rows, Ids, relations and Active/DeletedAt/CreatedAt/UpdatedAt
# are kept, and a second run changes nothing.
#
# Run from the directory that contains the compose file and .env, like backup-hostinger.sh:
#   ./scripts/apply-demo-workspace-names.sh            # dry run: prints counts, then rolls back
#   ./scripts/apply-demo-workspace-names.sh --apply    # commits (take a verified backup first)
# Hostinger Docker Manager keeps the project as /docker/<project>/docker-compose.yaml:
#   COMPOSE_FILE=docker-compose.yaml COMPOSE_PROJECT_NAME=<project> SQL_FILE=./DemoWorkspaceNames.sql \
#     ./apply-demo-workspace-names.sh --apply

COMPOSE_FILE=${COMPOSE_FILE:-compose.prod.yaml}
ENV_FILE=${ENV_FILE:-.env}
SQL_FILE=${SQL_FILE:-$(dirname "$0")/../src/Infrastructure/Common/Helpers/DemoWorkspaceNames.sql}

case "${1:-}" in
  "") finish="ROLLBACK" ;;
  --apply) finish="COMMIT" ;;
  *) echo "Usage: $0 [--apply]" >&2; exit 2 ;;
esac

for required in "${COMPOSE_FILE}" "${ENV_FILE}" "${SQL_FILE}"; do
  if [[ ! -f "${required}" ]]; then
    echo "Missing ${required}; run this script from the Cpnucleo deploy directory or set COMPOSE_FILE/ENV_FILE/SQL_FILE." >&2
    exit 1
  fi
done

# Parse only the keys this script needs from dotenv syntax without executing it.
read_dotenv='
import ast
import sys

with open(sys.argv[1], encoding="utf-8") as f:
    for raw in f:
        line = raw.strip()
        if line.startswith("export "):
            line = line[len("export "):].lstrip()
        if not line or line.startswith("#") or "=" not in line:
            continue
        key, value = (part.strip() for part in line.split("=", 1))
        if key not in {"POSTGRES_USER", "POSTGRES_DB"}:
            continue
        if len(value) >= 2 and value[0] == value[-1] and value[0] in {chr(34), chr(39)}:
            try:
                value = ast.literal_eval(value)
            except Exception:
                value = value[1:-1]
        print(f"{key}\t{value}")
'
while IFS=$'\t' read -r key value; do
  case "${key}" in
    POSTGRES_USER) DOTENV_POSTGRES_USER=${value} ;;
    POSTGRES_DB) DOTENV_POSTGRES_DB=${value} ;;
  esac
done < <(python3 -c "${read_dotenv}" "${ENV_FILE}")

POSTGRES_USER=${POSTGRES_USER:-${DOTENV_POSTGRES_USER:-}}
POSTGRES_DB=${POSTGRES_DB:-${DOTENV_POSTGRES_DB:-}}
: "${POSTGRES_USER:?POSTGRES_USER must be set in ${ENV_FILE}}"
: "${POSTGRES_DB:?POSTGRES_DB must be set in ${ENV_FILE}}"

compose=(docker compose --env-file "${ENV_FILE}" -f "${COMPOSE_FILE}")
if [[ -z "$("${compose[@]}" ps --status running --quiet db)" ]]; then
  echo "No running db container for ${COMPOSE_FILE}; set COMPOSE_PROJECT_NAME/COMPOSE_FILE to match the deployed project." >&2
  exit 1
fi

echo "Demo workspace names: ${finish} on database ${POSTGRES_DB}."
{
  echo "BEGIN;"
  echo "SET LOCAL lock_timeout = '5s';"
  cat "${SQL_FILE}"
  echo "${finish};"
} | "${compose[@]}" exec -T db psql -U "${POSTGRES_USER}" -d "${POSTGRES_DB}" -v ON_ERROR_STOP=1 --quiet

if [[ "${finish}" == "ROLLBACK" ]]; then
  echo "Dry run only; nothing was changed. Re-run with --apply after a verified backup."
fi
