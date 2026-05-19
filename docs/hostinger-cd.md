# Hostinger Continuous Deployment

This repository deploys production containers to Hostinger through the Hostinger VPS Docker Manager API.

## Deployment model

1. `Main Release` builds and publishes all four application images to GHCR.
2. The workflow merges multi-arch manifests tagged with the current commit SHA.
3. `deploy-hostinger` posts `compose.prod.yaml` and a generated environment payload to Hostinger Docker Manager.
4. Hostinger recreates the `cpnucleo` Docker project using immutable image tags.
5. The workflow verifies Hostinger action completion, expected containers, logs, and public smoke URLs.

The deployment uses immutable tags such as:

```text
ghcr.io/jonathanperis/cpnucleo-web-api:sha-<git-sha>
ghcr.io/jonathanperis/cpnucleo-identity-api:sha-<git-sha>
ghcr.io/jonathanperis/cpnucleo-grpc-server:sha-<git-sha>
ghcr.io/jonathanperis/cpnucleo-web-client:sha-<git-sha>
```

Do not use `latest` for Hostinger production rollouts.

## GitHub repository secrets

Required:

| Secret | Purpose |
| --- | --- |
| `HOSTINGER_API_TOKEN` | Bearer token for Hostinger Developer API. |
| `HOSTINGER_VPS_ID` | Hostinger VPS id. |
| `HOSTINGER_PROJECT_NAME` | Docker Manager project name, currently `cpnucleo-prod`. |
| `HOSTINGER_ENV_BASE64` | Base64-encoded production env file. |

Smoke-test URLs:

| Secret | Purpose |
| --- | --- |
| `CPNUCLEO_WEB_URL` | Public WebClient URL. |
| `CPNUCLEO_API_URL` | Public REST API base URL; `/healthz` is appended. |
| `CPNUCLEO_IDENTITY_URL` | Public Identity API base URL; `/healthz` is appended. |
| `CPNUCLEO_GRPC_HEALTH_URL` | Optional public gRPC health URL if exposed over HTTP. |

Legacy Azure deployment is manual opt-in only through `workflow_dispatch` input `deploy_azure=true`.

## Preparing `HOSTINGER_ENV_BASE64`

Create the production env file on a secure machine. Start from `.env.hostinger.example`, fill real values, and remove placeholders.

The deploy script overrides these four image variables on every run, so the base env may omit them or contain old values:

```text
CPNUCLEO_WEB_API_IMAGE
CPNUCLEO_IDENTITY_API_IMAGE
CPNUCLEO_GRPC_SERVER_IMAGE
CPNUCLEO_WEB_CLIENT_IMAGE
```

The base env must include these active keys:

```text
CPNUCLEO_WEB_HOST
CPNUCLEO_API_HOST
CPNUCLEO_IDENTITY_HOST
CPNUCLEO_GRPC_HOST
CPNUCLEO_GRAFANA_HOST
CPNUCLEO_GRAFANA_BASIC_AUTH_USERS
GRAFANA_ADMIN_USER
GRAFANA_ADMIN_PASSWORD
WebApiBaseUrl
TRAEFIK_NETWORK
TRAEFIK_CERT_RESOLVER
ASPNETCORE_ENVIRONMENT
ASPNETCORE_FORWARDEDHEADERS_ENABLED
POSTGRES_USER
POSTGRES_PASSWORD
POSTGRES_DB
DB_CONNECTION_STRING
Jwt__SigningKey
OTEL_EXPORTER_OTLP_ENDPOINT
OTEL_METRIC_EXPORT_INTERVAL
```

Encode the file without printing it to CI logs:

```bash
base64 -w0 .env.hostinger > /tmp/hostinger-env-base64.txt
gh secret set HOSTINGER_ENV_BASE64 --repo jonathanperis/cpnucleo < /tmp/hostinger-env-base64.txt
rm -f /tmp/hostinger-env-base64.txt
```

## Manual rollout

After the CD PR is merged and secrets are configured:

```bash
gh workflow run "Main Release" --ref main -f deploy_hostinger=true -f deploy_azure=false
gh run list --workflow "Main Release" --branch main --limit 5
```

Watch the selected run until `deploy-hostinger` completes.

## Automatic rollout

On every push to `main`, `deploy-hostinger` runs automatically after:

- build/test matrix succeeds;
- amd64 and arm64 images are pushed;
- multi-arch manifests are created;
- container health-check job succeeds.

## Rollback

Rollback is deterministic because Hostinger receives explicit `sha-<git-sha>` image tags.

Current rollback options:

1. Revert the bad commit on `main` and let the release workflow deploy the reverted SHA.
2. Temporarily run `scripts/deploy-hostinger-docker-manager.sh` with `GITHUB_SHA` set to a known-good SHA and the same Hostinger secrets.

A future improvement can add a dedicated `hostinger-rollback.yml` workflow with a required `deploy_sha` input.

## Safety notes

- Do not print `HOSTINGER_ENV_BASE64` or decoded env contents in logs.
- Do not run broad VPS cleanup commands from CD.
- Do not prune Docker volumes or images automatically; the VPS also runs Traefik and Hermes-related services.
- Treat Hostinger action `success` as necessary but not sufficient; always verify containers and public endpoints.
