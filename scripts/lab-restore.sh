#!/usr/bin/env bash
set -euo pipefail

# Operates only on the local lab service. Never accepts a production connection.
compose=(docker compose -f compose.lab.yaml)
probe="restore_probe_$(openssl rand -hex 6)"
"${compose[@]}" exec -T db createdb -U learner "${probe}"
cleanup() { "${compose[@]}" exec -T db dropdb -U learner "${probe}" >/dev/null; }
trap cleanup EXIT

"${compose[@]}" exec -T db pg_dump -U learner --format=custom --no-owner --no-acl cpnucleo_lab |
  "${compose[@]}" exec -T db pg_restore -U learner --exit-on-error -d "${probe}"

counts='SELECT (SELECT count(*) FROM "Projects"), (SELECT count(*) FROM "Users"), (SELECT count(*) FROM "Assignments");'
source_counts=$("${compose[@]}" exec -T db psql -U learner -d cpnucleo_lab -Atqc "${counts}")
restored_counts=$("${compose[@]}" exec -T db psql -U learner -d "${probe}" -Atqc "${counts}")
if [[ "${source_counts}" != "${restored_counts}" ]]; then
  printf '%s\n' 'Restore verification failed: row counts differ.' >&2
  exit 1
fi
printf '%s\n' 'Restore verified: project, user and assignment counts match. Source data was preserved.'
