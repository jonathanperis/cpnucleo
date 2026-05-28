#!/usr/bin/env bash
set -euo pipefail

HOSTINGER_API="${HOSTINGER_API:-https://developers.hostinger.com/api/vps/v1}"
HOSTINGER_UA="${HOSTINGER_UA:-Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 Chrome/125 Safari/537.36}"
PROJECT_OWNER_REPO="jonathanperis/cpnucleo"

required=(
  HOSTINGER_API_TOKEN
  HOSTINGER_VPS_ID
  HOSTINGER_PROJECT_NAME
  HOSTINGER_ENV_BASE64
  GITHUB_SHA
)

for key in "${required[@]}"; do
  if [[ -z "${!key:-}" ]]; then
    echo "Missing required environment variable: ${key}" >&2
    exit 1
  fi
done

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

workdir="$(mktemp -d)"
trap 'rm -rf "${workdir}"' EXIT
base_env="${workdir}/hostinger.base.env"
final_env="${workdir}/hostinger.final.env"
payload_file="${workdir}/payload.json"
response_file="${workdir}/response.json"
action_file="${workdir}/action.json"
containers_file="${workdir}/containers.json"
logs_file="${workdir}/logs.json"

redact() {
  sed -E \
    -e 's/(Password=)[^;[:space:]]+/\1<redacted>/gI' \
    -e 's/(password["=: ]+)[^,"[:space:]]+/\1<redacted>/gI' \
    -e 's/(Jwt__SigningKey["=: ]+)[^,"[:space:]]+/\1<redacted>/gI' \
    -e 's/(Authorization: Bearer )[A-Za-z0-9._-]+/\1<redacted>/gI'
}

api_curl() {
  curl -sS \
    -H "Authorization: Bearer ${HOSTINGER_API_TOKEN}" \
    -H "Accept: application/json" \
    -A "${HOSTINGER_UA}" \
    "$@"
}

api_curl_json() {
  curl -sS \
    -H "Authorization: Bearer ${HOSTINGER_API_TOKEN}" \
    -H "Accept: application/json" \
    -H "Content-Type: application/json" \
    -A "${HOSTINGER_UA}" \
    "$@"
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
  echo "Verifying image manifest: ${image}"
  docker manifest inspect "${image}" >/dev/null
}

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
    "ASPNETCORE_FORWARDEDHEADERS_ENABLED",
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

echo "Deploying ${HOSTINGER_PROJECT_NAME} to Hostinger VPS ${HOSTINGER_VPS_ID} with image tag ${tag}."
api_curl_json -X POST --data-binary "@${payload_file}" \
  "${HOSTINGER_API}/virtual-machines/${HOSTINGER_VPS_ID}/docker" > "${response_file}"

action_id="$(extract_action_id "${response_file}")"
if [[ -z "${action_id}" ]]; then
  echo "Hostinger deployment response did not include an action id. Sanitized response:" >&2
  redact < "${response_file}" >&2
  exit 1
fi

echo "Hostinger action id: ${action_id}"
terminal_state=""
for attempt in {1..60}; do
  api_curl "${HOSTINGER_API}/virtual-machines/${HOSTINGER_VPS_ID}/actions/${action_id}" > "${action_file}"
  terminal_state="$(json_value "${action_file}" 'state|status|data.state|data.status' || true)"
  echo "Hostinger action poll ${attempt}: ${terminal_state:-unknown}"
  case "${terminal_state,,}" in
    success|succeeded|finished|completed|done)
      break
      ;;
    error|failed|failure)
      echo "Hostinger action failed. Sanitized response:" >&2
      redact < "${action_file}" >&2
      exit 1
      ;;
  esac
  sleep 10
done

case "${terminal_state,,}" in
  success|succeeded|finished|completed|done) ;;
  *)
    echo "Timed out waiting for Hostinger action ${action_id}. Last sanitized response:" >&2
    redact < "${action_file}" >&2
    exit 1
    ;;
esac

containers_verified=false
for attempt in {1..24}; do
  api_curl "${HOSTINGER_API}/virtual-machines/${HOSTINGER_VPS_ID}/docker/${HOSTINGER_PROJECT_NAME}/containers" > "${containers_file}" || true
  if python3 - "${containers_file}" "${expected_containers[@]}" <<'PY'
import json, sys
path = sys.argv[1]
expected = set(sys.argv[2:])
try:
    data = json.load(open(path, encoding="utf-8"))
except Exception as exc:
    raise SystemExit(f"Could not parse Hostinger containers response: {exc}")
items = data.get("data", data) if isinstance(data, dict) else data
if isinstance(items, dict) and "containers" in items:
    items = items["containers"]
if not isinstance(items, list):
    raise SystemExit("Unexpected Hostinger containers response shape")
containers = {}
for item in items:
    if not isinstance(item, dict):
        continue
    name = item.get("name") or item.get("container_name")
    if name:
        containers[name] = item
missing = sorted(expected - containers.keys())
not_running = []
for name, item in containers.items():
    state = str(item.get("state") or item.get("status") or "").lower()
    health = str(item.get("health") or "").lower()
    if name in expected and "running" not in state and "up" not in state:
        not_running.append(f"{name}: {item.get('state') or item.get('status')}")
    if name in expected and health and health not in {"healthy", "none", "null", "starting"}:
        not_running.append(f"{name}: health={health}")
if missing or not_running:
    if missing:
        print("Missing expected containers: " + ", ".join(missing), file=sys.stderr)
    if not_running:
        print("Unhealthy/non-running containers: " + "; ".join(not_running), file=sys.stderr)
    raise SystemExit(1)
print(f"Hostinger containers verified: {len(expected)} expected containers present/running")
PY
  then
    containers_verified=true
    break
  fi
  echo "Hostinger containers not healthy yet; retrying (${attempt}/24)..."
  sleep 10
done
if [[ "${containers_verified}" != "true" ]]; then
  echo "Timed out waiting for Hostinger containers to become healthy." >&2
  exit 1
fi

api_curl "${HOSTINGER_API}/virtual-machines/${HOSTINGER_VPS_ID}/docker/${HOSTINGER_PROJECT_NAME}/logs" > "${logs_file}" || true
if grep -Eiq 'Unhandled exception|panic:|segmentation fault|no space left on device' "${logs_file}"; then
  echo "Potential startup failure markers found in Hostinger logs:" >&2
  redact < "${logs_file}" | tail -200 >&2
  exit 1
fi

echo "Hostinger deployment completed successfully for ${tag}."
