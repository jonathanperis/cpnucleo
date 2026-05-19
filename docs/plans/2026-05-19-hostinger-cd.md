# Hostinger CD Implementation Plan

> **For Hermes:** Use subagent-driven-development skill to implement this plan task-by-task.

**Goal:** Add GitHub Actions continuous deployment that redeploys the Hostinger Docker Manager project after the main release workflow publishes immutable GHCR image tags.

**Architecture:** Keep GitHub Actions as the image builder and release orchestrator. After all four service images are pushed and the multi-arch `sha-${{ github.sha }}` manifests exist, a deploy job calls Hostinger's Docker Manager API with `compose.prod.yaml` plus a generated environment payload that pins every service to that commit SHA. Secrets stay in GitHub Actions secrets; the workflow verifies Hostinger action success, container state, and live HTTPS health before reporting success.

**Tech Stack:** GitHub Actions, GHCR, Docker Buildx, Hostinger VPS Docker Manager API, Docker Compose production stack, Traefik public routing.

---

## Progress Snapshot

- **Repo/path inspected:** `/opt/data/github/jonathanperis/cpnucleo`
- **Branch/ref inspected:** `fix/hostinger-inline-prod-compose` at `9f8bf9b`; `origin/main` at `8fc78d8`
- **Working-tree state before writing this plan:** clean
- **Prior plans/reports consulted:** none found under this repo
- **Evidence checked:**
  - `.github/workflows/main-release.yml` already builds and pushes GHCR images as `sha-${{ github.sha }}` and merges multi-arch manifests.
  - `compose.prod.yaml` already consumes `CPNUCLEO_WEB_API_IMAGE`, `CPNUCLEO_IDENTITY_API_IMAGE`, `CPNUCLEO_GRPC_SERVER_IMAGE`, and `CPNUCLEO_WEB_CLIENT_IMAGE` env vars.
  - `.env.hostinger.example` documents immutable Hostinger image env vars and public Hostinger hosts.
  - Hostinger Docker Manager API supports create/replace project by posting `project_name`, `content`, and `environment`, then polling an action id.

| Lane | Planning progress | Evidence checked | Implementation status | Validation status |
| --- | --- | --- | --- | --- |
| Release workflow integration | planned | `main-release.yml` image jobs and dependencies | not started | not started |
| Hostinger API deploy script | planned | Hostinger API notes and compose/env shape | not started | not started |
| Secrets/env model | planned | `.env.hostinger.example` | not started | not started |
| Deployment verification | planned | Hostinger API endpoints and live public host variables | not started | not started |
| Documentation/runbook | planned | repo docs paths inspected | not started | not started |

**Overall planning progress:** Implementation plan is complete. Remaining work is to add the deploy script/workflow, configure GitHub secrets, test on a manual dispatch, then enable automatic deploy after main release.

---

## Recommended CD Strategy

Use **Hostinger Docker Manager API from GitHub Actions**, not SSH, as the default path.

Why:
- It matches how the project is currently hosted through Hostinger's Docker Manager.
- It avoids adding SSH/firewall/key management to the deployment path.
- It can atomically replace the Docker Manager project using the production compose and a generated env payload.
- It keeps the VPS shared infrastructure safer: we redeploy only the `cpnucleo` Docker project and do not run broad `docker system prune` or touch Traefik/Hermes containers.

Fallback only if the API is too limiting: SSH into the VPS and run `docker compose --env-file /opt/cpnucleo/.env -f compose.prod.yaml pull && docker compose ... up -d`, but that should be a second option because it bypasses Hostinger's project state.

---

## Required GitHub Secrets

Add these repository secrets before enabling the workflow:

- `HOSTINGER_API_TOKEN` — Hostinger developer API bearer token.
- `HOSTINGER_VPS_ID` — Hostinger virtual machine id from `GET /api/vps/v1/virtual-machines`.
- `HOSTINGER_PROJECT_NAME` — recommended: `cpnucleo`.
- `HOSTINGER_ENV_BASE64` — base64-encoded production env payload excluding the four image variables, or including placeholders that the deploy script overrides.

Optional but useful:

- `CPNUCLEO_WEB_URL` — `https://cpnucleo.jonathanperis.tech`.
- `CPNUCLEO_API_URL` — `https://api-cpnucleo.jonathanperis.tech`.
- `CPNUCLEO_IDENTITY_URL` — `https://identity-cpnucleo.jonathanperis.tech`.
- `CPNUCLEO_GRPC_HEALTH_URL` — if exposed via a usable HTTP health path.

Do not print decoded env payloads in Actions logs.

---

## Deployment Flow

1. Push/merge to `main` triggers `Main Release`.
2. `setup-build-test` verifies build and architecture tests.
3. `build-push-amd64` and `build-push-arm64` publish architecture-specific images.
4. `merge-manifest` creates multi-arch immutable tags:
   - `ghcr.io/jonathanperis/cpnucleo-web-api:sha-${{ github.sha }}`
   - `ghcr.io/jonathanperis/cpnucleo-identity-api:sha-${{ github.sha }}`
   - `ghcr.io/jonathanperis/cpnucleo-grpc-server:sha-${{ github.sha }}`
   - `ghcr.io/jonathanperis/cpnucleo-web-client:sha-${{ github.sha }}`
5. New `deploy-hostinger` job waits for `merge-manifest` and container tests.
6. The job verifies every immutable image manifest exists.
7. The job creates a final env payload by decoding `HOSTINGER_ENV_BASE64` and appending/overriding the four `CPNUCLEO_*_IMAGE` variables for the current SHA.
8. The job POSTs the project to Hostinger Docker Manager:
   - `project_name`: `cpnucleo`
   - `content`: raw GitHub URL to `compose.prod.yaml` at the exact commit SHA
   - `environment`: generated env payload
9. The job polls the Hostinger action until terminal.
10. The job verifies:
    - project exists in Hostinger project list;
    - expected containers are present/running;
    - logs do not show obvious startup failures;
    - public API/WebClient health endpoints return expected HTTP status.

---

## Task 1: Add a Hostinger deploy script

**Objective:** Create one reusable script that GitHub Actions can call without leaking secrets.

**Files:**
- Create: `scripts/deploy-hostinger-docker-manager.sh`

**Step 1: Create script skeleton**

Implement a Bash script with strict mode:

```bash
#!/usr/bin/env bash
set -euo pipefail

required=(
  HOSTINGER_API_TOKEN
  HOSTINGER_VPS_ID
  HOSTINGER_PROJECT_NAME
  HOSTINGER_ENV_BASE64
  GITHUB_SHA
)

for key in "${required[@]}"; do
  if [[ -z "${!key:-}" ]]; then
    echo "Missing required environment variable: $key" >&2
    exit 1
  fi
done
```

**Step 2: Add image tag generation**

```bash
TAG="sha-${GITHUB_SHA}"
WEB_API_IMAGE="ghcr.io/jonathanperis/cpnucleo-web-api:${TAG}"
IDENTITY_API_IMAGE="ghcr.io/jonathanperis/cpnucleo-identity-api:${TAG}"
GRPC_SERVER_IMAGE="ghcr.io/jonathanperis/cpnucleo-grpc-server:${TAG}"
WEB_CLIENT_IMAGE="ghcr.io/jonathanperis/cpnucleo-web-client:${TAG}"
COMPOSE_URL="https://raw.githubusercontent.com/jonathanperis/cpnucleo/${GITHUB_SHA}/compose.prod.yaml"
```

**Step 3: Verify manifests before deployment**

Run:

```bash
docker manifest inspect "$WEB_API_IMAGE" >/dev/null
docker manifest inspect "$IDENTITY_API_IMAGE" >/dev/null
docker manifest inspect "$GRPC_SERVER_IMAGE" >/dev/null
docker manifest inspect "$WEB_CLIENT_IMAGE" >/dev/null
```

Expected: all commands exit `0`.

**Step 4: Decode and rewrite env safely**

The script should:

- decode `HOSTINGER_ENV_BASE64` into a temp file;
- remove existing `CPNUCLEO_WEB_API_IMAGE=`, `CPNUCLEO_IDENTITY_API_IMAGE=`, `CPNUCLEO_GRPC_SERVER_IMAGE=`, `CPNUCLEO_WEB_CLIENT_IMAGE=` lines;
- append the four immutable image values for the current SHA;
- validate there are no active `CHANGE_ME` / `REPLACE_ME` placeholders in non-comment lines;
- never print the env file.

**Step 5: Build JSON payload through Python**

Avoid shell quoting bugs:

```bash
python3 - <<'PY' "$HOSTINGER_PROJECT_NAME" "$COMPOSE_URL" "$ENV_FILE" "$PAYLOAD_FILE"
import json, pathlib, sys
project, compose_url, env_path, payload_path = sys.argv[1:]
environment = pathlib.Path(env_path).read_text()
payload = {
    "project_name": project,
    "content": compose_url,
    "environment": environment,
}
pathlib.Path(payload_path).write_text(json.dumps(payload))
PY
```

**Step 6: POST to Hostinger and poll action**

Use `curl` with browser-like user-agent to avoid Cloudflare 1010:

```bash
HOSTINGER_API="https://developers.hostinger.com/api/vps/v1"
UA="Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 Chrome/125 Safari/537.36"

response=$(curl -sS -X POST \
  -H "Authorization: Bearer ${HOSTINGER_API_TOKEN}" \
  -H "Content-Type: application/json" \
  -H "Accept: application/json" \
  -A "$UA" \
  --data-binary "@${PAYLOAD_FILE}" \
  "${HOSTINGER_API}/virtual-machines/${HOSTINGER_VPS_ID}/docker")
```

Parse the action id from the response. If no action id is present, print sanitized response metadata and fail.

Poll:

```bash
GET /virtual-machines/${HOSTINGER_VPS_ID}/actions/${ACTION_ID}
```

until `success` or `error`.

**Step 7: Verify project containers**

After action success, call:

```bash
GET /virtual-machines/${HOSTINGER_VPS_ID}/docker/${HOSTINGER_PROJECT_NAME}/containers
GET /virtual-machines/${HOSTINGER_VPS_ID}/docker/${HOSTINGER_PROJECT_NAME}/logs
```

Fail if expected app containers are absent or obviously unhealthy. Redact connection strings/JWT/password-like values before printing logs.

**Step 8: Commit**

```bash
git add scripts/deploy-hostinger-docker-manager.sh
git commit -m "ci: add hostinger docker manager deploy script"
```

---

## Task 2: Add Hostinger deploy job to main release workflow

**Objective:** Deploy to Hostinger only after images are published and tested.

**Files:**
- Modify: `.github/workflows/main-release.yml`

**Step 1: Add workflow-level concurrency**

Add near the top:

```yaml
concurrency:
  group: cpnucleo-hostinger-release
  cancel-in-progress: false
```

This prevents overlapping Hostinger deployments from racing.

**Step 2: Add `deploy-hostinger` job**

Add after `merge-manifest` and before/alongside the current Azure `deploy-image` job:

```yaml
  deploy-hostinger:
    name: Deploy to Hostinger Docker Manager
    needs: [container-test, merge-manifest]
    runs-on: ubuntu-latest
    environment: production-hostinger
    env:
      HOSTINGER_API_TOKEN: ${{ secrets.HOSTINGER_API_TOKEN }}
      HOSTINGER_VPS_ID: ${{ secrets.HOSTINGER_VPS_ID }}
      HOSTINGER_PROJECT_NAME: ${{ secrets.HOSTINGER_PROJECT_NAME }}
      HOSTINGER_ENV_BASE64: ${{ secrets.HOSTINGER_ENV_BASE64 }}
      CPNUCLEO_WEB_URL: ${{ secrets.CPNUCLEO_WEB_URL }}
      CPNUCLEO_API_URL: ${{ secrets.CPNUCLEO_API_URL }}
      CPNUCLEO_IDENTITY_URL: ${{ secrets.CPNUCLEO_IDENTITY_URL }}
    steps:
      - name: Checkout Repository
        uses: actions/checkout@v6
        with:
          fetch-depth: 0

      - name: Login to GitHub Container Registry
        uses: docker/login-action@v4
        with:
          registry: ghcr.io
          username: ${{ github.actor }}
          password: ${{ secrets.GITHUB_TOKEN }}

      - name: Deploy Hostinger project
        run: scripts/deploy-hostinger-docker-manager.sh
```

**Step 3: Decide Azure fate**

Because the app is now running on Hostinger, choose one:

- **Preferred:** keep Azure jobs manual-only during transition, then remove after Hostinger CD is stable.
- **Alternative:** leave Azure deploy running in parallel until Hostinger has had a few successful deploys.

Implementation option for transition:

```yaml
  deploy-infra:
    if: ${{ github.event_name == 'workflow_dispatch' && inputs.deploy_azure == true }}
```

But this requires adding a `workflow_dispatch` boolean input. Do not remove Azure deployment in the same PR unless explicitly approved.

**Step 4: Commit**

```bash
git add .github/workflows/main-release.yml
git commit -m "ci: deploy releases to hostinger"
```

---

## Task 3: Add production environment assembly documentation

**Objective:** Make setup repeatable without exposing secrets.

**Files:**
- Modify: `.env.hostinger.example`
- Create: `docs/hostinger-cd.md`

**Step 1: Document secret generation**

In `docs/hostinger-cd.md`, include:

```bash
# On a secure local machine, prepare env from the production values.
# Do not commit the real .env.hostinger file.
base64 -w0 .env.hostinger > /tmp/hostinger-env-base64.txt
```

Then paste `/tmp/hostinger-env-base64.txt` into GitHub secret `HOSTINGER_ENV_BASE64`.

**Step 2: Document required active env keys**

List the keys that must exist in the decoded env:

- `CPNUCLEO_WEB_HOST`
- `CPNUCLEO_API_HOST`
- `CPNUCLEO_IDENTITY_HOST`
- `CPNUCLEO_GRPC_HOST`
- `CPNUCLEO_GRAFANA_HOST`
- `TRAEFIK_NETWORK`
- `TRAEFIK_CERT_RESOLVER`
- `ASPNETCORE_ENVIRONMENT`
- `ASPNETCORE_FORWARDEDHEADERS_ENABLED`
- `POSTGRES_USER`
- `POSTGRES_PASSWORD`
- `POSTGRES_DB`
- `DB_CONNECTION_STRING`
- `Jwt__SigningKey`
- `OTEL_EXPORTER_OTLP_ENDPOINT`

**Step 3: Commit**

```bash
git add .env.hostinger.example docs/hostinger-cd.md
git commit -m "docs: document hostinger cd setup"
```

---

## Task 4: Add live smoke verification

**Objective:** Prove deployment works from GitHub Actions, not just that Hostinger accepted the project update.

**Files:**
- Modify: `scripts/deploy-hostinger-docker-manager.sh`

**Step 1: Add curl helper**

```bash
check_url() {
  local name="$1"
  local url="$2"
  [[ -z "$url" ]] && return 0

  for attempt in {1..30}; do
    code=$(curl -k -sS -o /tmp/cpnucleo-smoke-body -w "%{http_code}" "$url" || true)
    if [[ "$code" =~ ^(200|301|302|401|404)$ ]]; then
      echo "$name smoke passed: HTTP $code"
      return 0
    fi
    echo "$name smoke attempt $attempt got HTTP $code; retrying..."
    sleep 10
  done

  echo "$name smoke failed for $url" >&2
  return 1
}
```

**Step 2: Verify public endpoints**

Suggested checks:

```bash
check_url "WebClient" "${CPNUCLEO_WEB_URL:-}"
check_url "WebApi health" "${CPNUCLEO_API_URL:+${CPNUCLEO_API_URL%/}/healthz}"
check_url "IdentityApi health" "${CPNUCLEO_IDENTITY_URL:+${CPNUCLEO_IDENTITY_URL%/}/healthz}"
```

For gRPC, use an HTTP health endpoint only if the public routing exposes one reliably; otherwise verify the container health/log state from Hostinger API.

**Step 3: Commit**

```bash
git add scripts/deploy-hostinger-docker-manager.sh
git commit -m "ci: verify hostinger deployment health"
```

---

## Task 5: Add a manual dry-run workflow path

**Objective:** Allow testing Hostinger deployment before turning it loose on every `main` push.

**Files:**
- Modify: `.github/workflows/main-release.yml`

**Step 1: Add workflow dispatch input**

```yaml
on:
  push:
    branches:
      - main
  workflow_dispatch:
    inputs:
      deploy_hostinger:
        description: Deploy current commit to Hostinger after images are published
        type: boolean
        default: true
```

**Step 2: Gate automatic deploy during rollout**

For the first PR, make deployment manual or main-only depending on confidence:

```yaml
    if: ${{ github.event_name == 'push' || inputs.deploy_hostinger == true }}
```

If you want a safer first pass, invert it:

```yaml
    if: ${{ github.event_name == 'workflow_dispatch' && inputs.deploy_hostinger == true }}
```

After one successful manual deployment, change to automatic on `main` push.

**Step 3: Commit**

```bash
git add .github/workflows/main-release.yml
git commit -m "ci: add manual hostinger deployment control"
```

---

## Task 6: Open PR and validate

**Objective:** Land the CD change through the repo's required PR workflow.

**Files:**
- No new files; Git/GitHub operation.

**Step 1: Sync and branch correctly**

```bash
git fetch origin main
git switch main
git pull --ff-only origin main
git switch -c ci/hostinger-cd
```

If the current Hostinger compose support branch is not merged yet, either base this work on that branch or merge/rebase after it lands. Do not duplicate Hostinger compose fixes in the CD PR unless required.

**Step 2: Run local checks**

```bash
bash -n scripts/deploy-hostinger-docker-manager.sh
dotnet test test/Architecture.Tests/
git diff --check
```

**Step 3: Push and open PR**

```bash
git push -u origin HEAD
gh pr create --base main --head ci/hostinger-cd \
  --title "ci: deploy releases to Hostinger" \
  --body-file /tmp/hostinger-cd-pr.md
```

**Step 4: Watch PR checks**

```bash
gh pr checks --watch
```

**Step 5: Merge only after approval**

Repo uses rebase-only merges:

```bash
gh pr merge --rebase --delete-branch
```

Ask for explicit approval before merging.

---

## Task 7: First production rollout

**Objective:** Verify one real Hostinger deployment end-to-end.

**Step 1: Confirm secrets exist**

Use GitHub UI or `gh secret list` to confirm:

```bash
gh secret list --repo jonathanperis/cpnucleo
```

Do not print secret values.

**Step 2: Trigger manual run**

```bash
gh workflow run "Main Release" --ref main -f deploy_hostinger=true
```

**Step 3: Watch release**

```bash
gh run list --workflow "Main Release" --branch main --limit 5
gh run watch <run-id>
```

**Step 4: Verify live endpoints after success**

```bash
curl -I https://cpnucleo.jonathanperis.tech
curl -I https://api-cpnucleo.jonathanperis.tech/healthz
curl -I https://identity-cpnucleo.jonathanperis.tech/healthz
```

**Step 5: Verify Hostinger state**

Use Docker Manager API project/container endpoints to verify the `cpnucleo` project is running and image tags contain the deployed `sha-...`.

---

## Rollback Plan

Rollback should be deterministic because production uses immutable SHA tags.

1. Identify the last known-good Git SHA.
2. Re-run the Hostinger deploy script with `GITHUB_SHA=<good-sha>` and the same env payload, or trigger a workflow dispatch with a `deploy_sha` input if implemented later.
3. Verify Hostinger action success, container state, and live endpoints.

Optional follow-up: add a dedicated `hostinger-rollback.yml` workflow with a required `deploy_sha` input after the main CD path is stable.

---

## Risks and Mitigations

- **Hostinger action success can hide container failures.** Mitigation: verify project containers/logs and public endpoints after polling action success.
- **Secrets can leak in logs.** Mitigation: never echo env payloads; redact logs; pass JSON via temp file and `--data-binary @file`.
- **Overlapping deploys can race.** Mitigation: workflow `concurrency` with `cancel-in-progress: false`.
- **Compose content limit/env limit.** Mitigation: use raw compose URL instead of inline full compose content; keep env payload under 8192 chars.
- **GHCR private image access.** Mitigation: ensure packages are public or configure VPS/Hostinger with registry auth if Docker Manager supports it. Validate by pulling the pinned image on the VPS or observing successful container creation.
- **Azure pipeline still deploys stale/duplicate production.** Mitigation: keep Azure until Hostinger is proven, then make Azure manual-only or remove in a separate PR.

---

## Acceptance Criteria

- GitHub Actions publishes all four `sha-${{ github.sha }}` multi-arch images.
- Hostinger deployment job updates the `cpnucleo` Docker Manager project using the exact commit's `compose.prod.yaml`.
- Runtime env pins all four services to the current commit's immutable image tags.
- Hostinger action reaches success.
- Expected containers are running.
- Public WebClient/API/Identity URLs pass smoke checks.
- Secrets are not printed in Actions logs.
- PR checks pass and the CD PR is merged with the repo's rebase-only strategy after explicit approval.
