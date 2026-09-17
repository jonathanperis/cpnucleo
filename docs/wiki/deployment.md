# Deployment

Cpnucleo uses Docker Compose for containerized deployment and GitHub Actions for CI/CD, with final deployment to Hostinger Docker Manager.

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

The base configuration defines all services with pre-built GHCR images:

| Service | Image | Internal Port | External Port |
|---------|-------|--------------|---------------|
| webapi1-cpnucleo | ghcr.io/jonathanperis/cpnucleo-web-api:latest | 5000 | 5100 |
| webapi2-cpnucleo | ghcr.io/jonathanperis/cpnucleo-web-api:latest | 5000 | 5111 |
| identityapi-cpnucleo | ghcr.io/jonathanperis/cpnucleo-identity-api:latest | 5010 | 5200 |
| grpcserver-cpnucleo | ghcr.io/jonathanperis/cpnucleo-grpc-server:latest | 5020/5021 | 5300/5301 |
| webclient-cpnucleo | ghcr.io/jonathanperis/cpnucleo-web-client:latest | 5030 | 5400 |
| db | PostgreSQL (tag in Compose) | 5432 | 5432 |
| nginx | nginx:1.27-alpine | 9999 | 9999 |
| otel-collector | otel/opentelemetry-collector-contrib:0.126.0 | 4317/4318 | None in base |
| otel-lgtm | grafana/otel-lgtm:0.11.10 | 3000/4317/4318 | None in base |

All API services depend on `db` being healthy and the collector being started. This legacy topology uses initial SQL on an empty volume; it has no one-shot migration service. Apply subsequent migrations explicitly as described in [Database](../database/). Its base healthchecks still assume `curl` in application images; use the lab configuration for the verified local startup path.

### Development Override (`compose.override.yaml`)

```bash
cp .env.example .env
docker compose -f compose.yaml -f compose.override.yaml up --build
```

Differences from base:

- Builds from source using Dockerfiles in `src/`
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

Use `.env.hostinger.example` as the production configuration checklist. `PUBLIC_*` WebClient URLs are compiled into the static assets when images are built; changing only a running container's environment does not retarget them.

---

## Dockerfiles

The three .NET services have multi-stage Dockerfiles with the following options. WebClient instead builds Astro with Node/Bun versions pinned in its Dockerfile and serves static output through its Node preview server on port 5030; it does not use the .NET publishing flags.

### Build Arguments

| Argument | Description | Dev Value | Prod Value |
|----------|-------------|-----------|------------|
| `AOT` | Enable Native AOT compilation | false | false |
| `TRIM` | Legacy name: enable ReadyToRun/self-contained publishing, not IL trimming | false | true |
| `EXTRA_OPTIMIZE` | Aggressive optimizations (remove symbols, disable debugger, invariant globalization) | false | true |
| `BUILD_CONFIGURATION` | .NET build configuration | Debug | Release |
| `ASPNETCORE_ENVIRONMENT` | Runtime environment | Development | Production |

`DB_CONNECTION_STRING`, JWT settings and CORS origins are runtime environment configuration, not image build arguments. Native AOT remains an experiment; the standard release leaves it disabled. `EventSourceSupport` and HTTP activity propagation remain enabled for telemetry.

### Build Stages

1. **base** -- pinned .NET 10 ASP.NET runtime image
2. **build** -- pinned .NET 10 SDK with clang/zlib for AOT support; restores, builds
3. **publish** -- Publishes with configured optimizations
4. **final** -- Copies published output to runtime image

### Platform Support

The release workflow builds `linux/amd64` and `linux/arm64/v8` images. Multi-arch `latest` and immutable `sha-${GITHUB_SHA}` manifests wait for both builds and the amd64 container checks. Arm64 images are built, but the workflow does not run an arm64 container smoke suite.

---

## NGINX Reverse Proxy

NGINX load-balances traffic across two WebApi instances:

```nginx
upstream api {
    least_conn;
    server webapi1-cpnucleo:5000;
    server webapi2-cpnucleo:5000;
}

server {
    listen 9999;
    location / {
        proxy_buffering off;
        proxy_read_timeout 60s;
        proxy_http_version 1.1;
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
- **gzip compression** -- level 5, minimum 256 bytes
- **keepalive_timeout: 0** -- persistent connections disabled for stateless APIs
- **server_tokens: off** -- hides NGINX version
- **access_log: off** -- disabled for performance
- **epoll** event model with multi_accept
- **SSE** response buffering disabled, with a 60-second upstream read timeout

---

## GitHub Actions CI/CD

### Build Check (`build-check.yml`)

Triggered on pull requests to main and manual dispatch.

**Jobs:**

1. **Setup, Build & Test** (matrix: WebApi, GrpcServer, IdentityApi, WebClient)
   - Checkout repository
   - Setup .NET SDK (from global.json)
   - Restore dependencies
   - Build application (Debug, no AOT/Trim)
    - Run architecture tests for backend matrix entries
    - Run all five backend suites plus disposable outbox/persistence experiments in the WebApi entry
    - Typecheck, test generated markup, build and audit WebClient in its matrix entry

2. **Container Healthcheck Test** (depends on build)
   - Build Docker image from source
    - Start the selected service and dependencies through `compose.lab.yaml`
    - Poll API `/readyz` (WebClient `/healthz`) up to 20 times with 5-second intervals
    - Fail if health check does not return 200
    - WebApi lane also exercises lab login/persistence and logical restore

3. **Documentation and dependency audit**
    - Check source/documentation contracts, audit dependencies, build Pages and check generated internal links

### Main Release (`main-release.yml`)

Triggered on push to main and manual dispatch.

**Jobs:**

1. **Setup, Build & Test** -- Build services with `TRIM=true`, `EXTRA_OPTIMIZE=true`, `BUILD_CONFIGURATION=Release`; run architecture and backend behavioral suites plus WebClient checks. Behavioral tests explicitly disable publishing flags/self-contained output so test hosts can load the production assemblies.

2. **Build & Push Docker Images** (depends on test)
    - Build `linux/amd64` images tagged `sha-${GITHUB_SHA}-amd64`
   - Build `linux/arm64/v8` images tagged `sha-${GITHUB_SHA}-arm64` and `latest-arm64`

3. **Container Healthcheck Test** (depends on push)
    - Pull the exact immutable amd64 images into the disposable lab configuration without rebuilding
    - Verify API readiness, WebClient health and lab login/persistence

4. **Deploy to Hostinger Docker Manager** (depends on amd64 images + container health checks)
   - Deploy the production Compose project through `scripts/deploy-hostinger-docker-manager.sh`
   - Uses Hostinger project secrets plus immutable `sha-${GITHUB_SHA}-amd64` GHCR image tags
    - Verifies the public WebClient, WebApi, IdentityApi, and gRPC health routes after deployment

5. **Merge Multi-arch Manifest** (depends on both architecture builds and container checks)
    - Publish `sha-${GITHUB_SHA}` and `latest` manifests; this lane can run independently of Hostinger deployment

Manual dispatch can disable Hostinger deployment with `deploy_hostinger=false` while retaining image publication. Publishing and deploying remain explicit maintainer operations.

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

---

## Environment Variables

### Required (`.env`)

| Variable | Description | Example |
|----------|-------------|---------|
| `POSTGRES_USER` | PostgreSQL username | postgres |
| `POSTGRES_PASSWORD` | PostgreSQL password | postgres |
| `POSTGRES_DB` | Database name | cpnucleo |
| `DB_CONNECTION_STRING` | Full Npgsql connection string | Host=db;Username=postgres;... |
| `OTEL_EXPORTER_OTLP_ENDPOINT` | OpenTelemetry collector endpoint | http://otel-lgtm:4317 |
| `OTEL_METRIC_EXPORT_INTERVAL` | Metric export interval (ms) | 5000 |

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

The deployment script receives database/JWT/Grafana settings inside `HOSTINGER_ENV_BASE64`, not a separate `DB_CONNECTION_STRING` Actions secret. Encoding is transport formatting, not encryption. See the example environment file for `CPNUCLEO_*_IMAGE`, `CPNUCLEO_*_HOST`, `Jwt__*`, `Cors__AllowedOrigins__*`, `CPNUCLEO_ADMIN_LOGINS`, Grafana and Traefik variables.

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

Production also attaches public-facing services to `${TRAEFIK_NETWORK:-traefik}`, which must already exist. Minimum/recommended VPS sizing is documented at the top of `compose.prod.yaml`.

## Source of truth

- [Standalone production Compose](https://github.com/jonathanperis/cpnucleo/blob/main/compose.prod.yaml)
- [Production environment template](https://github.com/jonathanperis/cpnucleo/blob/main/.env.hostinger.example)
- [Release workflow](https://github.com/jonathanperis/cpnucleo/blob/main/.github/workflows/main-release.yml)
- [PR checks](https://github.com/jonathanperis/cpnucleo/blob/main/.github/workflows/build-check.yml)
