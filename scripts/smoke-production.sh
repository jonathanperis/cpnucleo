#!/usr/bin/env bash
set -euo pipefail

workdir="$(mktemp -d)"
trap 'rm -rf "${workdir}"' EXIT

check_url() {
  local name="$1"
  local url="$2"
  local allowed_codes="${3:-200}"

  if [[ -z "${url}" ]]; then
    echo "${name} smoke skipped: URL is empty"
    return 0
  fi

  for attempt in {1..30}; do
    code="$(curl --connect-timeout 5 --max-time 15 -sS -o "${workdir}/smoke-body" -w "%{http_code}" "${url}" || true)"
    if [[ ",${allowed_codes}," == *",${code},"* ]]; then
      echo "${name} smoke passed: HTTP ${code}"
      return 0
    fi

    echo "${name} smoke attempt ${attempt} got HTTP ${code}; retrying..."
    sleep 10
  done

  echo "${name} smoke failed for ${url}" >&2
  if [[ -s "${workdir}/smoke-body" ]]; then
    echo "Last response body preview:" >&2
    head -c 500 "${workdir}/smoke-body" >&2 || true
    echo >&2
  fi
  return 1
}

CPNUCLEO_WEB_URL="${CPNUCLEO_WEB_URL:-https://cpnucleo.jonathanperis.tech/}"
CPNUCLEO_API_URL="${CPNUCLEO_API_URL:-https://api-cpnucleo.jonathanperis.tech}"
CPNUCLEO_IDENTITY_URL="${CPNUCLEO_IDENTITY_URL:-https://identity-cpnucleo.jonathanperis.tech}"
CPNUCLEO_GRPC_HEALTH_URL="${CPNUCLEO_GRPC_HEALTH_URL:-https://grpc-cpnucleo.jonathanperis.tech/healthz}"

check_url "WebClient" "${CPNUCLEO_WEB_URL:-}" "200,301,302"
if [[ -n "${CPNUCLEO_API_URL:-}" ]]; then
  check_url "WebApi health" "${CPNUCLEO_API_URL%/}/healthz" "200"
fi
if [[ -n "${CPNUCLEO_IDENTITY_URL:-}" ]]; then
  check_url "IdentityApi health" "${CPNUCLEO_IDENTITY_URL%/}/healthz" "200"
fi
if [[ -n "${CPNUCLEO_GRPC_HEALTH_URL:-}" ]]; then
  check_url "Grpc health" "${CPNUCLEO_GRPC_HEALTH_URL}" "200"
fi

echo "Production smoke tests completed successfully."
