# Deployment

Cpnucleo uses Docker Compose for containerized deployment and GitHub Actions for CI/CD, with final deployment to Hostinger Docker Manager.

---

## Toolchain

`global.json` pins SDK `10.0.401` with `rollForward: latestMinor`: any installed .NET 10 SDK at feature band 4xx or later works, but a 10.0.1xx SDK cannot satisfy it (roll-forward never goes backwards). Node.js comes from `.nvmrc` and Bun is pinned to 1.4.2. Container builds need Docker with Compose v2 and BuildKit.

---

## Docker Compose Configurations

Use `compose.lab.yaml` for the isolated learning stack, the base/development pair for the load-balanced development example, and `compose.prod.yaml` **alone** for production. Layering the base file into production retains its published ports.

### Recommended learning lab (`compose.lab.yaml`)

```sh
docker compose -f compose.lab.yaml up --build -d
docker compose -f compose.lab.yaml run --rm seed
```

The minimal stack contains PostgreSQL, a one-shot migrator, WebApi, IdentityApi and WebClient. Add `--profile full` for gRPC and `--profile observability` for Grafana LGTM. Published lab ports bind to loopback; PostgreSQL uses 15432. See [Getting Started](../getting-started/) for the service map and disposable seed profiles.

### Base (`compose.yaml`)

The base configuration defines all services with pre-built GHCR images (exact tags live in the Compose file):

| Service | Image | Internal Port | External Port |
|---------|-------|--------------|---------------|
| migrate-cpnucleo | ghcr.io/jonathanperis/cpnucleo-web-api:latest (`--migrate-database`) | — | — |
| webapi1-cpnucleo | ghcr.io/jonathanperis/cpnucleo-web-api:latest | 5000 | 5100 |
| webapi2-cpnucleo | ghcr.io/jonathanperis/cpnucleo-web-api:latest | 5000 | 5111 |
| identityapi-cpnucleo | ghcr.io/jonathanperis/cpnucleo-identity-api:latest | 5010 | 5200 |
| grpcserver-cpnucleo | ghcr.io/jonathanperis/cpnucleo-grpc-server:latest | 5020/5021 | 5300/5301 |
| webclient-cpnucleo | ghcr.io/jonathanperis/cpnucleo-web-client:latest | 5030 | 5400 |
| db | PostgreSQL | 5432 | 5432 |
| nginx | NGINX (Alpine) | 9999 | 9999 |
| otel-collector | OpenTelemetry Collector Contrib | 4317/4318 | None in base |
| otel-lgtm | Grafana LGTM | 3000/4317/4318 | None in base |

APIs wait for a healthy `db` and a successful one-shot `migrate-cpnucleo` run; the initial SQL still bootstraps an empty volume. Healthchecks use tools that exist in the runtime images: bash's `/dev/tcp` for the .NET apps (the ASP.NET runtime image has no `curl`) and BusyBox `wget` for the Alpine WebClient. NGINX starts only after both WebApi instances are healthy. Local collector telemetry is labelled `deployment.environment=development` unless the collector container sets `CPNUCLEO_DEPLOYMENT_ENVIRONMENT`.

### Development Override (`compose.override.yaml`)

```bash
cp .env.example .env
docker compose -f compose.yaml -f compose.override.yaml up --build
```

Differences from base:

- Builds from source using Dockerfiles in `src/` (the migrator builds the same WebApi image, so it never pulls a stale GHCR image)
- Build args: `AOT=false`, `TRIM=false`, `EXTRA_OPTIMIZE=false`, `BUILD_CONFIGURATION=Debug`
- Publishes Grafana on port 3000 and the collector's OTLP endpoints on 4317/4318; these mappings are not restricted to loopback
- Application-service limits: 0.4 CPU, 100MB; infrastructure has separate limits

### Standalone Production (`compose.prod.yaml`)

```bash
docker compose --env-file .env -f compose.prod.yaml up -d
```

Production behavior:

- Long-running services use `restart: always`; the one-shot migrator uses `restart: "no"`
- Resource reservations: 0.25 CPU / 256MB per API, 0.50 CPU / 512MB per DB
- Resource limits: 0.50 CPU / 512MB per API, 1.0 CPU / 1GB for DB
- JSON logging with rotation: 10MB max size, 3 files retained
- No build step; production image variables such as `CPNUCLEO_WEB_API_IMAGE` are required and should point at immutable GHCR tags (for example `sha-...`)
- A successful `--migrate-database` run is required before API startup; no automatic demo/bulk seeding
- No application/database host ports; Traefik owns public TLS/host routing on an existing external network, and internal NGINX balances the two WebApi instances
- A collector forwards OTLP traces, metrics and logs to the persistent LGTM stack; Grafana requires configured credentials and proxy basic authentication

#### Per-service environment

There is no shared `env_file`. Compose still reads `.env` (or `--env-file`) to interpolate `${...}`, but each container receives only what it uses:

| Container | Variables |
|-----------|-----------|
| WebApi ×2, GrpcServer | `ASPNETCORE_ENVIRONMENT`, `ASPNETCORE_FORWARDEDHEADERS_ENABLED=true` (set in Compose), `DB_CONNECTION_STRING`, `Jwt__MetadataAddress` (IdentityApi's internal discovery URL, set in Compose), `CPNUCLEO_ADMIN_LOGINS`, `Cors__AllowedOrigins__0`, `OTEL_EXPORTER_OTLP_ENDPOINT`, `OTEL_METRIC_EXPORT_INTERVAL`, `OTEL_TRACES_SAMPLER`, `OTEL_TRACES_SAMPLER_ARG` |
| IdentityApi | The API set above plus `Jwt__SigningKey` (encrypts its stored signing and Data Protection keys) and optional `Jwt__SigningPrivateKey` (pins one RS256 key instead of the rotating ring) |
| migrate-cpnucleo | `ASPNETCORE_ENVIRONMENT`, `DB_CONNECTION_STRING` |
| WebClient | `OTEL_EXPORTER_OTLP_HTTP_ENDPOINT`, `OTEL_SERVICE_NAME`, `OTEL_METRIC_EXPORT_INTERVAL` |
| db | `POSTGRES_USER`, `POSTGRES_PASSWORD`, `POSTGRES_DB` |
| otel-lgtm | Grafana admin credentials |
| otel-collector | `CPNUCLEO_DEPLOYMENT_ENVIRONMENT=production`, `CPNUCLEO_HOST_PROVIDER=hostinger` |

Forwarded headers are enabled explicitly because per-IP rate limiting sits behind Traefik → NGINX. Optional variables default to empty, which the applications treat as "not configured". `PUBLIC_*` WebClient URLs are compiled into the static assets when images are built; they are not passed at runtime because they would have no effect.

#### Container hardening

The .NET APIs, the migrator and WebClient run with `no-new-privileges`, `cap_drop: [ALL]`, a read-only root filesystem and a `/tmp` tmpfs. They listen on unprivileged ports as non-root users (`app` / `node`). PostgreSQL and NGINX keep their default capabilities because their entrypoints switch users and own data directories.

#### Network exposure

Only WebClient, IdentityApi, GrpcServer and NGINX join the external Traefik network. The LGTM container stays on the private Compose network, so its OTLP receivers (4317/4318) are not reachable from other stacks on the shared Traefik network. Grafana's UI is routed by Traefik to NGINX port 3000, which proxies (including WebSockets) to `otel-lgtm:3000` behind Traefik basic authentication and Grafana's own login.

#### Health and restarts

Application healthchecks probe `/healthz` every minute (10-second probes during the first-minute start period). Docker records unhealthy status but does **not** restart an unhealthy container by itself: `restart: always` reacts only when the process exits. An "autoheal" sidecar would need the Docker socket, which is equivalent to root on the VPS; this public deployment deliberately does not mount `docker.sock` into any container. Instead, failed health blocks deployment (see rollback below), NGINX re-resolves upstreams when containers are recreated, and an operator restarts an unhealthy service through Docker Manager.

Use `.env.hostinger.example` as the production configuration checklist.

---

## Dockerfiles

### .NET services

| Argument | Description | Dev Value | Prod Value |
|----------|-------------|-----------|------------|
| `AOT` | Enable Native AOT compilation (experiment) | false | false |
| `TRIM` | Legacy name: enable ReadyToRun/self-contained publishing, not IL trimming | false | true |
| `EXTRA_OPTIMIZE` | Aggressive optimizations (remove symbols, disable debugger, invariant globalization) | false | true |
| `BUILD_CONFIGURATION` | .NET build configuration | Debug | Release |
| `ASPNETCORE_ENVIRONMENT` | Runtime environment baked as the image default | Development | Production |

Build stages:

1. **base** -- pinned .NET 10 ASP.NET runtime image, non-root `app` user
2. **build** -- pinned .NET 10 SDK on the build machine's architecture (`FROM --platform=$BUILDPLATFORM`). It restores and runs a **single** `dotnet publish -r linux-<x64|arm64>` (no separate build pass that compiles twice). `clang`/`zlib1g-dev` are installed only when `AOT=true`
3. **final** -- copies the published output into the runtime image

Cross-publishing from the build architecture is supported by ReadyToRun: the SDK downloads the Crossgen2 package for the build host and targets `TARGETARCH`, so local multi-arch builds avoid emulating the SDK. Native AOT cross-architecture builds need a matching native toolchain, so `AOT=true` should be built on a runner of the target architecture; the release already uses native amd64 and arm64 runners.

No secrets are baked into images: `DB_CONNECTION_STRING`, JWT keys and CORS origins are runtime environment configuration only. `EXTRA_OPTIMIZE` keeps `UseSystemResourceKeys=false` so production exception messages stay readable, and keeps `EventSourceSupport` and HTTP activity propagation enabled for telemetry. The image `HEALTHCHECK` probes `/healthz` every minute with the same bash `/dev/tcp` request used by Compose.

### WebClient

WebClient builds Astro with the Node/Bun versions pinned in its Dockerfile and serves static output through its Node preview server (`node scripts/preview.mjs`, which loads the OpenTelemetry bootstrap) on port 5030. Bun is used only in build stages to honor `bun.lock`; the runtime image is plain Node with production dependencies. The `PUBLIC_WEBAPI_BASE_URL` and `PUBLIC_IDENTITY_API_BASE_URL` build arguments default to the local lab URLs (`http://localhost:5100/api`, `http://localhost:5200/api`); the release workflow passes production URLs explicitly. `PUBLIC_IDENTITY_API_ISSUER` is the expected JWT `iss` value, not a URL the browser calls, so its default matches the IdentityApi `Jwt:Issuer` configured in every appsettings file.

### Platform Support

The release workflow builds `linux/amd64` on `ubuntu-latest` and `linux/arm64` on native `ubuntu-24.04-arm` runners (no QEMU). Each architecture is pushed only under an immutable `sha-${GITHUB_SHA}-<arch>` tag and smoke-tested on its own native runner with the same container checks before the multi-arch `sha-${GITHUB_SHA}` and `latest` manifests are published.

---

## NGINX Reverse Proxy

NGINX load-balances traffic across two WebApi instances and fronts the Grafana UI:

```nginx
resolver 127.0.0.11 valid=10s ipv6=off;

upstream api {
    zone api 64k;
    least_conn;
    server webapi1-cpnucleo:5000 resolve;
    server webapi2-cpnucleo:5000 resolve;
    keepalive 16;
}

server {
    listen 9999;
    location / {
        proxy_buffering off;
        proxy_read_timeout 60s;
        proxy_http_version 1.1;
        proxy_set_header Connection "";
        proxy_pass http://api;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }
}
```

### Configuration Highlights

- **least_conn** load balancing -- sends requests to the server with fewest active connections
- **Docker DNS re-resolution** -- `resolver 127.0.0.11` with `zone` + `server ... resolve` (NGINX OSS ≥ 1.27.3) picks up recreated containers' new IPs without restarting NGINX
- **Upstream keepalive** -- pooled HTTP/1.1 connections to the APIs (`proxy_http_version 1.1` and an empty `Connection` header)
- **Client keepalive** -- `keepalive_timeout 65s` lets Traefik reuse connections to NGINX
- **gzip compression** -- level 5, minimum 256 bytes
- **server_tokens: off** -- hides NGINX version
- **access_log: off** -- disabled for performance
- **epoll** event model with multi_accept
- **SSE** response buffering disabled, with a 60-second upstream read timeout
- **Grafana gateway** -- `listen 3000` proxies to `otel-lgtm:3000` with WebSocket upgrade support

`compose.prod.yaml` inlines `nginx.conf`, `otel-collector.yaml` and the two init SQL files as Compose `configs` (Docker Manager deploys a single file). `OperationsConfigurationTests` fails if an inlined copy drifts from its source file; edit the source file and copy it into the `configs:` block with `$` doubled to `$$`.

---

## GitHub Actions CI/CD

### Build Check (`build-check.yml`)

Triggered on pull requests to main and manual dispatch. A newer push to the same PR cancels the running check. Every job has a timeout and checkouts are shallow.

**Jobs:**

1. **Setup, Build & Test** (matrix: WebApi, GrpcServer, IdentityApi, WebClient)
   - Restore and build each .NET host (Debug, no AOT/Trim)
   - The WebApi entry runs `dotnet test cpnucleo.slnx` once: all five suites, Architecture included, with `XPlat Code Coverage`. ReportGenerator merges the Cobertura files and writes a coverage summary to the job summary, listing any test project without `coverlet.collector`
   - The WebApi entry also runs the disposable outbox/persistence experiments
   - Typecheck, test generated markup, build and audit WebClient in its matrix entry

2. **Upload coverage to Codecov** -- uploads the merged report with GitHub OIDC (`use_oidc: true`, `id-token: write` on this job only). It never fails CI, but an unsuccessful upload is reported as a warning in the job summary (for example while the repository is not yet activated on codecov.io, or on fork PRs without OIDC)

3. **Container Healthcheck Test** (depends on build)
   - `scripts/ci-container-test.sh <service>` builds the lab images, starts the service through `compose.lab.yaml` and polls API `/readyz` (WebClient `/healthz`) up to 30 times at 5-second intervals
   - WebApi lane also exercises lab login/persistence and logical restore

4. **Release Image Build** (matrix: WebApi, IdentityApi, GrpcServer) -- builds the release Dockerfile configuration (`Release`, `TRIM=true`, `EXTRA_OPTIMIZE=true`, amd64) without pushing, reusing the release layer cache, so ReadyToRun/self-contained regressions fail before main

5. **Documentation and dependency audit**
    - Check source/documentation contracts, audit dependencies, build Pages and check generated internal links

### Main Release (`main-release.yml`)

Triggered on push to main and manual dispatch. Every job has a timeout; permissions are granted per job.

**Jobs:**

1. **Setup, Build & Test** -- Build services with `TRIM=true`, `EXTRA_OPTIMIZE=true`, `BUILD_CONFIGURATION=Release`; run every backend suite once plus WebClient checks. Behavioral tests explicitly disable publishing flags/self-contained output so test hosts can load the production assemblies.

2. **Build & Push amd64 / arm64** (in parallel, native runners)
   - Push only `sha-${GITHUB_SHA}-amd64` / `sha-${GITHUB_SHA}-arm64`
   - GitHub Actions layer cache per image and architecture (`type=gha,scope=<image>-<arch>`)
   - SBOM and maximal provenance attestations (`sbom: true`, `provenance: mode=max`) attached to each image

3. **Container Healthcheck Test** (amd64) and **Container Healthcheck Test arm64**
   - Pull the exact immutable images into the disposable lab configuration on a runner of the same architecture, assert the image architecture, and verify API readiness, WebClient health and lab login/persistence with the shared `scripts/ci-container-test.sh`

4. **Vulnerability Scan amd64** -- Trivy scans each amd64 image, uploads CRITICAL/HIGH findings to GitHub code scanning (SARIF, `security-events: write` on this job only) and to the job summary, and fails only on CRITICAL vulnerabilities that have a fixed version

5. **Deploy to Hostinger Docker Manager** (depends on amd64 images, amd64 container checks and the scan)
   - Deploy the production Compose project through `scripts/deploy-hostinger-docker-manager.sh` with automatic rollback (below)
   - Uses Hostinger project secrets plus immutable `sha-${GITHUB_SHA}-amd64` GHCR image tags
   - Verifies the public WebClient, WebApi, IdentityApi (health plus OpenID discovery and JWKS), and gRPC health routes after deployment; a smoke failure triggers `deploy-hostinger-docker-manager.sh --rollback`

6. **Merge Multi-arch Manifest** (depends on both builds, both container checks and the scan)
   - Publish `sha-${GITHUB_SHA}` and `latest` manifests, then sign the manifest digest keylessly with cosign (`id-token: write` on this job only). This lane runs independently of Hostinger deployment

Manual dispatch can disable Hostinger deployment with `deploy_hostinger=false` while retaining image publication. Publishing and deploying remain explicit maintainer operations.

### Supply chain verification

Verify a published manifest signature and inspect its attestations:

```sh
cosign verify ghcr.io/jonathanperis/cpnucleo-web-api:latest \
  --certificate-identity https://github.com/jonathanperis/cpnucleo/.github/workflows/main-release.yml@refs/heads/main \
  --certificate-oidc-issuer https://token.actions.githubusercontent.com
docker buildx imagetools inspect ghcr.io/jonathanperis/cpnucleo-web-api:latest --format '{{ json .SBOM }}'
docker buildx imagetools inspect ghcr.io/jonathanperis/cpnucleo-web-api:latest --format '{{ json .Provenance }}'
```

The Trivy gate is intentionally narrow (fixable CRITICAL only) so base-image noise does not block releases; HIGH findings stay visible in code scanning. Renovate pins Docker base images to digests so rebuilds are reproducible and image updates arrive as reviewable PRs.

### Documentation and security workflows

- `deploy.yml` publishes `docs/out/` to GitHub Pages on main/manual dispatch through a pinned reusable workflow. It is separate from the application release.
- `codeql.yml` analyzes C#, JavaScript/TypeScript and GitHub Actions on PRs, main, a weekly schedule and manual dispatch.

### Hostinger Deployment Targets

| Surface | Public URL | Backing service |
|---------|------------|-----------------|
| WebClient | `https://cpnucleo.jonathanperis.tech/` | `webclient-cpnucleo` |
| WebApi | `https://api-cpnucleo.jonathanperis.tech/` | `webapi1-cpnucleo` / `webapi2-cpnucleo` |
| IdentityApi | `https://identity-cpnucleo.jonathanperis.tech/` | `identityapi-cpnucleo` |
| gRPC health | `https://grpc-cpnucleo.jonathanperis.tech/healthz` | `grpcserver-cpnucleo` |

### Deployment rollback

`scripts/deploy-hostinger-docker-manager.sh` uses the Hostinger VPS API in four steps:

1. **Capture** -- reads the currently deployed project (`GET .../docker/{project}`): its environment, including the `CPNUCLEO_*_IMAGE` tags. When the four images share one `sha-<commit>` tag, the rollback target is `compose.prod.yaml` at that commit; otherwise the captured compose content is reused if it fits Hostinger's 8192-character limit. The state is written with `0600` permissions to `$RUNNER_TEMP` and never printed. A first deploy (project missing) logs that rollback is unavailable.
2. **Deploy** -- submits `compose.prod.yaml` at `GITHUB_SHA` with the new image tags and polls the Hostinger action.
3. **Verify** -- polls containers until every expected container is running, healthy, and the application containers run exactly the `sha-${GITHUB_SHA}-amd64` images (so smoke tests cannot pass against old containers), then scans logs for startup failure markers.
4. **Roll back** -- if the action fails or times out, health polling times out, an image does not match, or the log check fails, the captured project is redeployed, its containers are verified against the previous images, and the script exits non-zero. If the subsequent production smoke tests fail, the workflow runs `--rollback`, which replays the same captured state.

Rollback lines are prefixed with `[rollback]` and raised as GitHub error annotations ("Deployment rolled back" or "Rollback failed"), and a successful rollback is added to the job summary. Rollback restores images and configuration only: additive migrations already applied by the new migrator stay in the database, which is why production migrations must remain backward compatible. A database-level undo needs the latest backup, so take one before every deploy (see below); the pipeline cannot run commands on the VPS itself.

The repository's Actions policy allows only GitHub-owned actions and an explicit allowlist (enforced by `OperationsConfigurationTests`), so cosign, Trivy and ReportGenerator are installed as pinned tools in `run:` steps: a checksum-verified cosign binary (`scripts/ci-install-cosign.sh`), a digest-pinned Trivy container and a versioned .NET tool.

Before deploying, the workflow verifies the keyless cosign signature of every `sha-<commit>-amd64` image (signed by the build jobs of this workflow on `main`). The `latest` and `sha-<commit>` multi-arch manifests are published only from `main` and only after the deploy succeeded (or was intentionally skipped), so `latest` never points at a release production rejected.

---

## Backups and restore verification

`scripts/backup-hostinger.sh` runs on the VPS from the project directory (for Docker Manager: `COMPOSE_FILE=docker-compose.yaml COMPOSE_PROJECT_NAME=<project>`). Nothing schedules it automatically; install a cron entry on the VPS, for example a nightly run plus one right before planned releases:

```cron
15 3 * * * cd /docker/cpnucleo && COMPOSE_FILE=docker-compose.yaml COMPOSE_PROJECT_NAME=cpnucleo /opt/cpnucleo/scripts/backup-hostinger.sh >> /var/log/cpnucleo-backup.log 2>&1
45 3 * * 0 /opt/cpnucleo/scripts/verify-backup.sh /opt/backups/cpnucleo >> /var/log/cpnucleo-backup.log 2>&1
```

The script:

1. Fails unless a running `db` container is found for the configured Compose project (`--env-file "$ENV_FILE"` is passed to every Compose call).
2. Dumps PostgreSQL in custom format and verifies the archive with `pg_restore --list` (the TOC must include `__EFMigrationsHistory`) before anything is checksummed.
3. Archives the compose file, `.env` and only the optional config files that exist (Docker Manager projects inline them).
4. Writes `SHA256SUMS` last, so its presence marks a complete backup.
5. Copies the backup off-host with `rsync` over SSH when `BACKUP_REMOTE` is set (`BACKUP_SSH_OPTIONS` adds SSH options). A failed copy fails the run but does not skip retention.
6. Deletes local timestamped backups older than `BACKUP_RETENTION_DAYS`. Retention runs after every verified backup and never after a failed dump, so repeated failures cannot age out the last good backups.

`scripts/verify-backup.sh [backup-dir]` proves restorability: it checks `SHA256SUMS` and the config archive, starts a throwaway `postgres:16.15` container with `--network none` and a random password, restores the latest complete dump with `--exit-on-error`, and prints `__EFMigrationsHistory` contents and row counts for key tables. It accepts no host or connection string and refuses to run commands in any container it did not create, so it cannot touch production. Run it after backups (for example weekly from cron) and alert on a non-zero exit.

### Renaming existing demo data

A database seeded before the workspace names still shows Bogus text ("monitor transmitting back-end"). `scripts/apply-demo-workspace-names.sh` applies the shared [`DemoWorkspaceNames.sql`](../database/#seed-and-recovery-tools) through `psql` in the `db` container. Nothing runs it automatically. Without arguments it is a dry run: it prints how many rows it would change and rolls back. Take and verify a backup first, then commit with `--apply`:

```sh
./scripts/backup-hostinger.sh && ./scripts/verify-backup.sh
COMPOSE_FILE=docker-compose.yaml COMPOSE_PROJECT_NAME=cpnucleo SQL_FILE=./DemoWorkspaceNames.sql ./apply-demo-workspace-names.sh
COMPOSE_FILE=docker-compose.yaml COMPOSE_PROJECT_NAME=cpnucleo SQL_FILE=./DemoWorkspaceNames.sql ./apply-demo-workspace-names.sh --apply
```

It runs in one transaction with a 5-second lock timeout and touches only rows that still carry generated text, so people's rows and the demo account are kept and a repeated run changes nothing. On the full dataset (about 1.37 million rows) it takes two to three minutes on a laptop, mostly maintaining the trigram search indexes, and locks the rows it rewrites meanwhile, so run it at a quiet time. Open lists pick the new names up through the 15-second SSE refresh. To undo it, restore the backup.

---

## Environment Variables

### Required (`.env`)

| Variable | Description | Example |
|----------|-------------|---------|
| `POSTGRES_USER` | PostgreSQL username | postgres |
| `POSTGRES_PASSWORD` | PostgreSQL password | postgres |
| `POSTGRES_DB` | Database name | cpnucleo |
| `DB_CONNECTION_STRING` | Full Npgsql connection string | Host=db;Username=postgres;... |
| `Jwt__SigningKey` | IdentityApi key-encryption secret for its stored keys (at least 32 bytes). Changing it makes the stored keys unreadable: new ones are created and everyone signs in again | openssl rand -base64 64 |
| `OTEL_EXPORTER_OTLP_ENDPOINT` | OpenTelemetry collector endpoint | http://otel-collector:4317 |

### Optional

| Variable | Description | Default |
|----------|-------------|---------|
| `Jwt__SigningPrivateKey` | Optional PEM RS256 key that IdentityApi signs with instead of its rotating key ring | empty (key ring) |
| `Identity__ServiceClients__{id}__Secret` / `__Login` / `__Scopes` | Optional client-credentials clients acting as a service account | none |
| `OTEL_TRACES_SAMPLER` / `OTEL_TRACES_SAMPLER_ARG` | Standard OpenTelemetry sampler selection, honored by the .NET hosts | `parentbased_always_on` / `1.0` |
| `OTEL_METRIC_EXPORT_INTERVAL` | Metric export interval (ms) | 30000 |
| `Cors__AllowedOrigins__0` | Browser origin allowed by WebApi/IdentityApi | `https://cpnucleo.jonathanperis.tech` |
| `CPNUCLEO_ADMIN_LOGINS` | Comma-separated admin logins. IdentityApi issues admin claims from it; WebApi and GrpcServer re-check it during session validation. Empty means the default: the seeded `demo@cpnucleo.local` account; a list replaces it | `demo@cpnucleo.local` |
| `BACKUP_DIR` / `BACKUP_RETENTION_DAYS` / `BACKUP_REMOTE` | Backup location, local retention and optional rsync target | `/opt/backups/cpnucleo` / 14 / empty |

The .NET hosts sample with `ParentBased(AlwaysOn)` unless `OTEL_TRACES_SAMPLER` is set, and `/healthz` and `/readyz` are excluded from ASP.NET Core traces.

### GitHub Secrets (for CI/CD)

| Secret | Purpose |
|--------|---------|
| `GITHUB_TOKEN` | GHCR authentication (automatic) |
| `HOSTINGER_API_TOKEN` | Hostinger API authentication |
| `HOSTINGER_VPS_ID` | Target Hostinger VPS identifier |
| `HOSTINGER_PROJECT_NAME` | Docker Manager project name |
| `HOSTINGER_ENV_BASE64` | Base64-encoded production `.env` payload |
| `CPNUCLEO_WEB_URL` | Public WebClient smoke-test URL |
| `CPNUCLEO_API_URL` | Public WebApi smoke-test URL |
| `CPNUCLEO_IDENTITY_URL` | Public IdentityApi smoke-test URL |
| `CPNUCLEO_GRPC_HEALTH_URL` | Public gRPC health smoke-test URL |

The deployment script receives database/JWT/Grafana settings inside `HOSTINGER_ENV_BASE64`, not a separate `DB_CONNECTION_STRING` Actions secret. Encoding is transport formatting, not encryption. See the example environment file for `CPNUCLEO_*_IMAGE`, `CPNUCLEO_*_HOST`, `Jwt__*`, `Cors__AllowedOrigins__*`, `CPNUCLEO_ADMIN_LOGINS`, Grafana and Traefik variables. Codecov uploads use OIDC and need no token.

---

## Network

The legacy and production stacks share the following internal network name (the isolated lab has its own project-scoped network):

```yaml
networks:
  default:
    name: cpnucleo_network
    driver: bridge
```

Service discovery uses Docker DNS (e.g., `db`, `webapi1-cpnucleo`).

Production also attaches public-facing services (WebClient, IdentityApi, GrpcServer, NGINX) to `${TRAEFIK_NETWORK:-traefik}`, which must already exist. Minimum/recommended VPS sizing is documented at the top of `compose.prod.yaml`.

## Source of truth

- [Standalone production Compose](https://github.com/jonathanperis/cpnucleo/blob/main/compose.prod.yaml)
- [Production environment template](https://github.com/jonathanperis/cpnucleo/blob/main/.env.hostinger.example)
- [Release workflow](https://github.com/jonathanperis/cpnucleo/blob/main/.github/workflows/main-release.yml)
- [PR checks](https://github.com/jonathanperis/cpnucleo/blob/main/.github/workflows/build-check.yml)
- [Deploy script](https://github.com/jonathanperis/cpnucleo/blob/main/scripts/deploy-hostinger-docker-manager.sh), [backup](https://github.com/jonathanperis/cpnucleo/blob/main/scripts/backup-hostinger.sh) and [restore verification](https://github.com/jonathanperis/cpnucleo/blob/main/scripts/verify-backup.sh)
