#!/usr/bin/env bash
# Starts one service of the disposable lab stack (compose.lab.yaml) and verifies
# it. Shared by the PR build check and the release pipeline (amd64 and arm64) so
# every image is held to the same container checks.
#
# Usage:
#   scripts/ci-container-test.sh <WebApi|IdentityApi|GrpcServer|WebClient> [--prebuilt] [--restore-check]
#
#   --prebuilt       Run already-published images without building. Requires the
#                    CPNUCLEO_*_IMAGE variables read by compose.lab.yaml; the
#                    migrator always uses CPNUCLEO_WEB_API_IMAGE.
#   --restore-check  WebApi only: also prove a logical dump restores (lab-restore.sh).
#
# Optional: CPNUCLEO_EXPECTED_ARCH=amd64|arm64 asserts the architecture of every
# pulled application image, so an arm64 job cannot silently test amd64 images.
# Only loopback lab URLs are used; this never touches production.
set -Eeuo pipefail

usage() {
  sed -n '2,16p' "$0" >&2
  exit 2
}

[[ $# -ge 1 ]] || usage
service="$1"
shift

mode=build
restore_check=false
for arg in "$@"; do
  case "${arg}" in
    --prebuilt) mode=prebuilt ;;
    --restore-check) restore_check=true ;;
    *) usage ;;
  esac
done

case "${service}" in
  WebApi) compose_service=webapi port=5100 route=readyz image_var=CPNUCLEO_WEB_API_IMAGE ;;
  IdentityApi) compose_service=identity port=5200 route=readyz image_var=CPNUCLEO_IDENTITY_API_IMAGE ;;
  GrpcServer) compose_service=grpc port=5301 route=readyz image_var=CPNUCLEO_GRPC_SERVER_IMAGE ;;
  WebClient) compose_service=webclient port=5400 route=healthz image_var=CPNUCLEO_WEB_CLIENT_IMAGE ;;
  *) echo "Unknown service '${service}'." >&2; usage ;;
esac

compose=(docker compose -f compose.lab.yaml --profile full)
if [[ "${mode}" == prebuilt ]]; then
  : "${CPNUCLEO_WEB_API_IMAGE:?--prebuilt requires CPNUCLEO_WEB_API_IMAGE (used by the migrator)}"
  : "${!image_var:?--prebuilt requires ${image_var}}"
  up_args=(--no-build --pull always)
else
  up_args=(--build)
fi

dump_diagnostics() {
  echo "::group::compose ps and logs" >&2
  "${compose[@]}" ps -a >&2 || true
  "${compose[@]}" logs --no-color --tail=200 >&2 || true
  echo "::endgroup::" >&2
}
trap dump_diagnostics ERR

# Polls instead of sleeping a fixed time: 30 attempts x 5 s covers cold starts.
wait_for_ok() {
  local name="$1" url="$2" status
  for attempt in {1..30}; do
    status=$(curl -sS -o /dev/null -w "%{http_code}" --max-time 10 "${url}") || status=000
    echo "${name} attempt ${attempt}: HTTP ${status}"
    if [[ "${status}" == 200 ]]; then
      echo "${name} passed with HTTP ${status}."
      return 0
    fi
    sleep 5
  done
  echo "${name} did not return HTTP 200 after 30 attempts: ${url}" >&2
  return 1
}

assert_architecture() {
  local image="$1" actual
  [[ -n "${CPNUCLEO_EXPECTED_ARCH:-}" ]] || return 0
  actual=$(docker image inspect --format '{{.Architecture}}' "${image}")
  if [[ "${actual}" != "${CPNUCLEO_EXPECTED_ARCH}" ]]; then
    echo "${image} is ${actual}; expected ${CPNUCLEO_EXPECTED_ARCH}." >&2
    return 1
  fi
  echo "${image} architecture: ${actual}"
}

"${compose[@]}" up "${up_args[@]}" -d "${compose_service}"
if [[ "${mode}" == prebuilt ]]; then
  assert_architecture "${!image_var}"
  if [[ "${service}" != WebClient ]]; then
    # The API services start only after the WebApi-image migrator succeeds.
    assert_architecture "${CPNUCLEO_WEB_API_IMAGE}"
  fi
fi
wait_for_ok "${service} /${route}" "http://localhost:${port}/${route}"

if [[ "${service}" == WebApi ]]; then
  "${compose[@]}" up "${up_args[@]}" -d identity
  if [[ "${mode}" == prebuilt ]]; then
    assert_architecture "${CPNUCLEO_IDENTITY_API_IMAGE:?--prebuilt WebApi checks also need CPNUCLEO_IDENTITY_API_IMAGE}"
  fi
  "${compose[@]}" run --rm seed
  wait_for_ok "IdentityApi /readyz" "http://localhost:5200/readyz"
  node scripts/smoke-lab.mjs
  if [[ "${restore_check}" == true ]]; then
    bash scripts/lab-restore.sh
  fi
fi

echo "Container checks passed for ${service} (${mode})."
