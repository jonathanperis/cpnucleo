#!/usr/bin/env bash
# Deploys compose.prod.yaml at GITHUB_SHA to Hostinger Docker Manager with the
# immutable sha-${GITHUB_SHA}-amd64 images, and rolls back automatically.
#
# Usage:
#   scripts/deploy-hostinger-docker-manager.sh             # deploy (default)
#   scripts/deploy-hostinger-docker-manager.sh --rollback  # redeploy the captured previous project
#
# Deploy mode first captures the currently deployed project (compose source and
# environment, including its image tags) through the Hostinger API and stores it
# with 0600 permissions in HOSTINGER_ROLLBACK_STATE (default:
# ${RUNNER_TEMP:-/tmp}/cpnucleo-hostinger-rollback.json). If the deploy action
# fails, health polling times out, a container runs an unexpected image, or the
# log check finds startup failures, the captured project is redeployed and the
# script exits non-zero. --rollback replays the same state file (the workflow
# uses it when production smoke tests fail after a successful deploy); it exits
# 0 only when the previous project is running healthy again.
set -euo pipefail

HOSTINGER_API="${HOSTINGER_API:-https://developers.hostinger.com/api/vps/v1}"
HOSTINGER_UA="${HOSTINGER_UA:-Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 Chrome/125 Safari/537.36}"
PROJECT_OWNER_REPO="jonathanperis/cpnucleo"
HOSTINGER_ROLLBACK_STATE="${HOSTINGER_ROLLBACK_STATE:-${RUNNER_TEMP:-/tmp}/cpnucleo-hostinger-rollback.json}"
# Hostinger's create-project API accepts at most 8192 characters of raw compose
# content; larger compose files must be referenced by URL.
HOSTINGER_CONTENT_LIMIT=8192

mode=deploy
case "${1:-}" in
  "") ;;
  --rollback) mode=rollback ;;
  *)
    echo "Usage: $0 [--rollback]" >&2
    exit 2
    ;;
esac

required=(HOSTINGER_API_TOKEN HOSTINGER_VPS_ID HOSTINGER_PROJECT_NAME)
if [[ "${mode}" == deploy ]]; then
  required+=(HOSTINGER_ENV_BASE64 GITHUB_SHA)
fi
for key in "${required[@]}"; do
  if [[ -z "${!key:-}" ]]; then
    echo "Missing required environment variable: ${key}" >&2
    exit 1
  fi
done

expected_containers=(
  otel-lgtm-cpnucleo
  otel-collector-cpnucleo
  webapi1-cpnucleo
  webapi2-cpnucleo
  identityapi-cpnucleo
  grpcserver-cpnucleo
  webclient-cpnucleo
  db-cpnucleo
  nginx-cpnucleo
)

umask 077
workdir="$(mktemp -d)"
trap 'rm -rf "${workdir}"' EXIT
base_env="${workdir}/hostinger.base.env"
final_env="${workdir}/hostinger.final.env"
payload_file="${workdir}/payload.json"
response_file="${workdir}/response.json"
action_file="${workdir}/action.json"
containers_file="${workdir}/containers.json"
logs_file="${workdir}/logs.json"
current_project_file="${workdir}/current-project.json"
images_file="${workdir}/expected-images.json"

log() { printf '[deploy] %s\n' "$*"; }
log_rollback() { printf '[rollback] %s\n' "$*"; }

# Never print raw API responses: they can echo the environment payload.
redact() {
  sed -E \
    -e 's/(Password=)[^;[:space:]]+/\1<redacted>/gI' \
    -e 's/(password["=: ]+)[^,"[:space:]]+/\1<redacted>/gI' \
    -e 's/(Jwt__Signing(Private|Public)?Key["=: ]+)[^,"[:space:]]+/\1<redacted>/gI' \
    -e 's/(BASIC_AUTH_USERS["=: ]+)[^,"[:space:]]+/\1<redacted>/gI' \
    -e 's/(Authorization: Bearer )[A-Za-z0-9._-]+/\1<redacted>/gI' \
    -e 's/("environment"[[:space:]]*:[[:space:]]*)"([^"\\]|\\.)*"/\1"<redacted>"/g'
}

api_curl() {
  curl -sS \
    -H "Authorization: Bearer ${HOSTINGER_API_TOKEN}" \
    -H "Accept: application/json" \
    -A "${HOSTINGER_UA}" \
    "$@"
}

api_curl_json() {
  api_curl -H "Content-Type: application/json" "$@"
}

json_value() {
  local path="$1"
  local expr="$2"
  python3 - "$path" "$expr" <<'PY'
import json, sys
path, expr = sys.argv[1:3]
with open(path, encoding="utf-8") as f:
    data = json.load(f)

def walk(obj, parts):
    if not parts:
        return obj
    part = parts[0]
    if isinstance(obj, dict):
        return walk(obj.get(part), parts[1:])
    return None

for candidate in expr.split('|'):
    value = walk(data, [p for p in candidate.split('.') if p])
    if value not in (None, ""):
        print(value)
        break
PY
}

extract_action_id() {
  local path="$1"
  python3 - "$path" <<'PY'
import json, sys
with open(sys.argv[1], encoding="utf-8") as f:
    data = json.load(f)

candidates = []

def collect(obj, path=()):
    if isinstance(obj, dict):
        for key, value in obj.items():
            key_path = path + (str(key),)
            lk = str(key).lower()
            if lk in {"actionid", "action_id", "id"} and isinstance(value, (str, int)):
                path_text = ".".join(part.lower() for part in key_path)
                action_distance = min(
                    (i for i, part in enumerate(key_path) if "action" in part.lower()),
                    default=10_000,
                )
                candidates.append((action_distance, -path_text.count("action"), len(key_path), str(value)))
            collect(value, key_path)
    elif isinstance(obj, list):
        for index, item in enumerate(obj):
            collect(item, path + (str(index),))

collect(data)
if candidates:
    candidates.sort()
    print(candidates[0][3])
PY
}

ensure_image_manifest() {
  local image="$1"
  log "Verifying image manifest: ${image}"
  docker manifest inspect "${image}" >/dev/null
}

# Writes the expected container -> image map used by verify_containers.
write_expected_images() {
  python3 - "${images_file}" "$@" <<'PY'
import json, sys
path, web_api, identity_api, grpc_server, web_client = sys.argv[1:]
images = {
    "webapi1-cpnucleo": web_api,
    "webapi2-cpnucleo": web_api,
    "identityapi-cpnucleo": identity_api,
    "grpcserver-cpnucleo": grpc_server,
    "webclient-cpnucleo": web_client,
}
json.dump({k: v for k, v in images.items() if v}, open(path, "w", encoding="utf-8"))
PY
}

# POSTs a project payload and waits for the Hostinger action. Returns non-zero
# on API errors, failed actions or timeouts.
submit_project() {
  local payload="$1" prefix="$2" action_id terminal_state=""
  if ! api_curl_json -X POST --data-binary "@${payload}" \
    "${HOSTINGER_API}/virtual-machines/${HOSTINGER_VPS_ID}/docker" > "${response_file}"; then
    echo "${prefix} Hostinger deploy request failed." >&2
    return 1
  fi

  action_id="$(extract_action_id "${response_file}" || true)"
  if [[ -z "${action_id}" ]]; then
    echo "${prefix} Hostinger deployment response did not include an action id. Sanitized response:" >&2
    redact < "${response_file}" >&2
    return 1
  fi

  echo "${prefix} Hostinger action id: ${action_id}"
  for attempt in {1..60}; do
    api_curl "${HOSTINGER_API}/virtual-machines/${HOSTINGER_VPS_ID}/actions/${action_id}" > "${action_file}" || true
    terminal_state="$(json_value "${action_file}" 'state|status|data.state|data.status' || true)"
    echo "${prefix} Hostinger action poll ${attempt}: ${terminal_state:-unknown}"
    case "${terminal_state,,}" in
      success|succeeded|finished|completed|done)
        return 0
        ;;
      error|failed|failure)
        echo "${prefix} Hostinger action failed. Sanitized response:" >&2
        redact < "${action_file}" >&2
        return 1
        ;;
    esac
    sleep 10
  done

  echo "${prefix} Timed out waiting for Hostinger action ${action_id}. Last sanitized response:" >&2
  redact < "${action_file}" >&2
  return 1
}

# Polls the project's containers until every expected container is running,
# healthy (when it has a healthcheck) and runs the expected image.
verify_containers() {
  local prefix="$1"
  for attempt in {1..24}; do
    api_curl "${HOSTINGER_API}/virtual-machines/${HOSTINGER_VPS_ID}/docker/${HOSTINGER_PROJECT_NAME}/containers" > "${containers_file}" || true
    if python3 - "${containers_file}" "${images_file}" "${expected_containers[@]}" <<'PY'
import json, re, sys
path, images_path = sys.argv[1:3]
expected = set(sys.argv[3:])
expected_images = json.load(open(images_path, encoding="utf-8"))
try:
    data = json.load(open(path, encoding="utf-8"))
except Exception as exc:
    raise SystemExit(f"Could not parse Hostinger containers response: {exc}")
items = data.get("data", data) if isinstance(data, dict) else data
if isinstance(items, dict) and "containers" in items:
    items = items["containers"]
if not isinstance(items, list):
    raise SystemExit("Unexpected Hostinger containers response shape")

def normalize(image):
    image = str(image or "").strip().split("@", 1)[0]
    return re.sub(r"^(docker\.io/)?", "", image)

containers = {}
for item in items:
    if not isinstance(item, dict):
        continue
    name = str(item.get("name") or item.get("container_name") or "").lstrip("/")
    if name:
        containers[name] = item
problems = [f"missing: {name}" for name in sorted(expected - containers.keys())]
for name in sorted(expected & containers.keys()):
    item = containers[name]
    state = str(item.get("state") or item.get("status") or "").lower()
    health = str(item.get("health") or "").lower()
    if "running" not in state and not state.startswith("up"):
        problems.append(f"{name}: {item.get('state') or item.get('status')}")
    if health not in {"", "healthy", "none", "null"}:
        problems.append(f"{name}: health={health}")
    want = expected_images.get(name)
    if want and normalize(item.get("image")) != normalize(want):
        problems.append(f"{name}: image {item.get('image')!r}, expected {want!r}")
if problems:
    print("Containers not ready: " + "; ".join(problems), file=sys.stderr)
    raise SystemExit(1)
print(f"Hostinger containers verified: {len(expected)} running/healthy, {len(expected_images)} on the expected images")
PY
    then
      return 0
    fi
    echo "${prefix} Hostinger containers not ready yet; retrying (${attempt}/24)..."
    sleep 10
  done
  echo "${prefix} Timed out waiting for Hostinger containers to become healthy on the expected images." >&2
  return 1
}

check_logs() {
  local prefix="$1"
  api_curl "${HOSTINGER_API}/virtual-machines/${HOSTINGER_VPS_ID}/docker/${HOSTINGER_PROJECT_NAME}/logs" > "${logs_file}" || true
  # Data migrations report their row counts (no secrets) through the migrator log.
  grep -oE 'Demo workspace names: [^"\\]*' "${logs_file}" | sort -u | sed "s/^/${prefix} /" || true
  if grep -Eiq 'Unhandled exception|panic:|segmentation fault|no space left on device' "${logs_file}"; then
    echo "${prefix} Potential startup failure markers found in Hostinger logs:" >&2
    redact < "${logs_file}" | tail -200 >&2
    return 1
  fi
}

# Captures the deployed project into HOSTINGER_ROLLBACK_STATE. Missing project
# (first deploy) or an unusable capture disables rollback with a clear warning.
capture_previous_project() {
  local status
  rm -f "${HOSTINGER_ROLLBACK_STATE}"
  status=$(api_curl -o "${current_project_file}" -w '%{http_code}' \
    "${HOSTINGER_API}/virtual-machines/${HOSTINGER_VPS_ID}/docker/${HOSTINGER_PROJECT_NAME}") || status=000
  if [[ "${status}" == 404 ]]; then
    log "No existing project ${HOSTINGER_PROJECT_NAME}; this is a first deploy and cannot be rolled back automatically."
    return 0
  fi
  if [[ "${status}" != 200 ]]; then
    echo "::warning title=Rollback unavailable::Could not read the deployed Hostinger project (HTTP ${status}); continuing without automatic rollback."
    return 0
  fi
  python3 - "${current_project_file}" "${HOSTINGER_ROLLBACK_STATE}" "${HOSTINGER_PROJECT_NAME}" \
    "${PROJECT_OWNER_REPO}" "${HOSTINGER_CONTENT_LIMIT}" <<'PY'
import json, re, sys
source, target, project, repo, limit = sys.argv[1:]
try:
    data = json.load(open(source, encoding="utf-8"))
except (OSError, ValueError) as error:
    print(f"::warning title=Rollback unavailable::The deployed project response could not be parsed ({type(error).__name__}); continuing without automatic rollback.")
    raise SystemExit(0)
data = data.get("data", data) if isinstance(data, dict) else {}
content = data.get("content") or ""
environment = data.get("environment") or ""
images = {}
for line in environment.splitlines():
    key, sep, value = line.strip().partition("=")
    if sep and key.strip() in {"CPNUCLEO_WEB_API_IMAGE", "CPNUCLEO_IDENTITY_API_IMAGE",
                               "CPNUCLEO_GRPC_SERVER_IMAGE", "CPNUCLEO_WEB_CLIENT_IMAGE"}:
        images[key.strip()] = value.strip().strip("'\"")
shas = {m.group(1) for v in images.values() if (m := re.search(r":sha-([0-9a-f]{40})(?:-[a-z0-9]+)?$", v))}
if len(shas) == 1 and len(images) == 4:
    # The compose file at the commit that produced the running images.
    sha = shas.pop()
    source_ref = f"https://raw.githubusercontent.com/{repo}/{sha}/compose.prod.yaml"
    description = f"compose.prod.yaml@{sha}"
elif content and len(content) <= int(limit):
    source_ref = content
    description = "captured compose content"
else:
    print("::warning title=Rollback unavailable::The deployed project has no consistent sha image tags "
          "and its compose content exceeds the Hostinger raw-content limit; continuing without automatic rollback.")
    raise SystemExit(0)
state = {
    "payload": {"project_name": project, "content": source_ref, "environment": environment},
    "images": images,
    "description": description,
}
with open(target, "w", encoding="utf-8") as f:
    json.dump(state, f)
print(f"[deploy] Captured previous project for rollback: {description}; images: "
      + ", ".join(f"{k}={v}" for k, v in sorted(images.items())))
PY
}

rollback() {
  local reason="$1"
  echo "::error title=Deployment failed::${reason}"
  if [[ ! -s "${HOSTINGER_ROLLBACK_STATE}" ]]; then
    log_rollback "No captured previous project; automatic rollback is unavailable. Manual intervention required."
    return 1
  fi
  local description
  description="$(json_value "${HOSTINGER_ROLLBACK_STATE}" 'description')"
  log_rollback "Redeploying the previous project (${description}) because: ${reason}"
  python3 - "${HOSTINGER_ROLLBACK_STATE}" "${payload_file}" <<'PY'
import json, sys
state = json.load(open(sys.argv[1], encoding="utf-8"))
json.dump(state["payload"], open(sys.argv[2], "w", encoding="utf-8"))
PY
  write_expected_images \
    "$(json_value "${HOSTINGER_ROLLBACK_STATE}" 'images.CPNUCLEO_WEB_API_IMAGE' || true)" \
    "$(json_value "${HOSTINGER_ROLLBACK_STATE}" 'images.CPNUCLEO_IDENTITY_API_IMAGE' || true)" \
    "$(json_value "${HOSTINGER_ROLLBACK_STATE}" 'images.CPNUCLEO_GRPC_SERVER_IMAGE' || true)" \
    "$(json_value "${HOSTINGER_ROLLBACK_STATE}" 'images.CPNUCLEO_WEB_CLIENT_IMAGE' || true)"
  if submit_project "${payload_file}" "[rollback]" && verify_containers "[rollback]"; then
    log_rollback "Rollback succeeded: the previous project (${description}) is running healthy again."
    echo "::error title=Deployment rolled back::${reason} The previous project (${description}) was restored."
    if [[ -n "${GITHUB_STEP_SUMMARY:-}" ]]; then
      printf '### Hostinger deployment rolled back\n\nReason: %s\n\nRestored: %s\n' "${reason}" "${description}" >> "${GITHUB_STEP_SUMMARY}"
    fi
    return 0
  fi
  log_rollback "Rollback FAILED. Production may be degraded; manual intervention required."
  echo "::error title=Rollback failed::Automatic rollback to ${description} failed; manual intervention required."
  return 1
}

if [[ "${mode}" == rollback ]]; then
  if rollback "Post-deploy verification (production smoke tests) failed."; then
    exit 0
  fi
  exit 1
fi

if [[ ! "${GITHUB_SHA}" =~ ^[0-9a-f]{40}$ ]]; then
  echo "GITHUB_SHA must be a full 40-character git SHA." >&2
  exit 1
fi

# Hostinger VPS currently runs amd64 images. The multi-arch sha-${GITHUB_SHA}
# manifest is created later by merge-manifest; deploy from the immutable amd64
# tags that already exist when this job starts.
tag="sha-${GITHUB_SHA}-amd64"
web_api_image="ghcr.io/jonathanperis/cpnucleo-web-api:${tag}"
identity_api_image="ghcr.io/jonathanperis/cpnucleo-identity-api:${tag}"
grpc_server_image="ghcr.io/jonathanperis/cpnucleo-grpc-server:${tag}"
web_client_image="ghcr.io/jonathanperis/cpnucleo-web-client:${tag}"
compose_url="https://raw.githubusercontent.com/${PROJECT_OWNER_REPO}/${GITHUB_SHA}/compose.prod.yaml"

ensure_image_manifest "${web_api_image}"
ensure_image_manifest "${identity_api_image}"
ensure_image_manifest "${grpc_server_image}"
ensure_image_manifest "${web_client_image}"

printf '%s' "${HOSTINGER_ENV_BASE64}" | base64 -d > "${base_env}"

python3 - "${base_env}" "${final_env}" \
  "${web_api_image}" "${identity_api_image}" "${grpc_server_image}" "${web_client_image}" <<'PY'
from pathlib import Path
import re, sys
base_path, final_path, web_api, identity_api, grpc_server, web_client = sys.argv[1:]
remove = {
    "CPNUCLEO_WEB_API_IMAGE",
    "CPNUCLEO_IDENTITY_API_IMAGE",
    "CPNUCLEO_GRPC_SERVER_IMAGE",
    "CPNUCLEO_WEB_CLIENT_IMAGE",
}
lines = []
for line in Path(base_path).read_text(encoding="utf-8").splitlines():
    stripped = line.strip()
    if stripped and not stripped.startswith("#") and "=" in stripped:
        key = stripped.split("=", 1)[0].strip()
        if key in remove:
            continue
    lines.append(line.rstrip("\r"))

lines.extend([
    f"CPNUCLEO_WEB_API_IMAGE={web_api}",
    f"CPNUCLEO_IDENTITY_API_IMAGE={identity_api}",
    f"CPNUCLEO_GRPC_SERVER_IMAGE={grpc_server}",
    f"CPNUCLEO_WEB_CLIENT_IMAGE={web_client}",
])

# compose.prod.yaml sets ASPNETCORE_FORWARDEDHEADERS_ENABLED itself and gives
# each container only the variables it needs.
required = {
    "CPNUCLEO_WEB_HOST",
    "CPNUCLEO_API_HOST",
    "CPNUCLEO_IDENTITY_HOST",
    "CPNUCLEO_GRPC_HOST",
    "CPNUCLEO_GRAFANA_HOST",
    "CPNUCLEO_GRAFANA_BASIC_AUTH_USERS",
    "GRAFANA_ADMIN_USER",
    "GRAFANA_ADMIN_PASSWORD",
    "TRAEFIK_NETWORK",
    "TRAEFIK_CERT_RESOLVER",
    "ASPNETCORE_ENVIRONMENT",
    "POSTGRES_USER",
    "POSTGRES_PASSWORD",
    "POSTGRES_DB",
    "DB_CONNECTION_STRING",
    "Jwt__SigningKey",
    "OTEL_EXPORTER_OTLP_ENDPOINT",
    "CPNUCLEO_WEB_API_IMAGE",
    "CPNUCLEO_IDENTITY_API_IMAGE",
    "CPNUCLEO_GRPC_SERVER_IMAGE",
    "CPNUCLEO_WEB_CLIENT_IMAGE",
}

active = {}
for line in lines:
    stripped = line.strip()
    if not stripped or stripped.startswith("#") or "=" not in stripped:
        continue
    key, value = stripped.split("=", 1)
    key = key.strip()
    active[key] = value
    if re.search(r"CHANGE_ME|REPLACE_ME", value):
        raise SystemExit(f"Refusing to deploy with placeholder value in {key}")

missing = sorted(required - active.keys())
if missing:
    raise SystemExit("Missing required env keys: " + ", ".join(missing))

# Without an explicit list the seeded demo account is the administrator; make that visible.
if not active.get("CPNUCLEO_ADMIN_LOGINS", "").strip():
    print("::notice title=Default administrator::CPNUCLEO_ADMIN_LOGINS is not set; demo@cpnucleo.local administers users and catalog data.")

text = "\n".join(lines).rstrip() + "\n"
if len(text) > 8192:
    raise SystemExit(f"Hostinger environment payload is {len(text)} bytes; limit is 8192")
Path(final_path).write_text(text, encoding="utf-8")
PY

python3 - "${HOSTINGER_PROJECT_NAME}" "${compose_url}" "${final_env}" "${payload_file}" <<'PY'
import json, pathlib, sys
project, compose_url, env_path, payload_path = sys.argv[1:]
environment = pathlib.Path(env_path).read_text(encoding="utf-8")
payload = {
    "project_name": project,
    "content": compose_url,
    "environment": environment,
}
pathlib.Path(payload_path).write_text(json.dumps(payload), encoding="utf-8")
PY

capture_previous_project
write_expected_images "${web_api_image}" "${identity_api_image}" "${grpc_server_image}" "${web_client_image}"

log "Deploying ${HOSTINGER_PROJECT_NAME} to Hostinger VPS ${HOSTINGER_VPS_ID} with image tag ${tag}."
if ! submit_project "${payload_file}" "[deploy]"; then
  rollback "The Hostinger deploy action for ${tag} failed or timed out." || true
  exit 1
fi

if ! verify_containers "[deploy]"; then
  rollback "Containers did not become healthy on the ${tag} images." || true
  exit 1
fi

if ! check_logs "[deploy]"; then
  rollback "Startup failure markers were found in the logs after deploying ${tag}." || true
  exit 1
fi

log "Hostinger deployment completed successfully for ${tag}; all application containers run sha-${GITHUB_SHA} images."
