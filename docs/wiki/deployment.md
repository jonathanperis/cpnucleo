# Deployment

Cpnucleo uses Docker Compose for containerized deployment and GitHub Actions for CI/CD, with final deployment to Azure Web Apps.

---

## Docker Compose Configurations

The project provides three compose configurations that can be layered:

### Base (`compose.yaml`)

The base configuration defines all services with pre-built GHCR images:

| Service | Image | Internal Port | External Port |
|---------|-------|--------------|---------------|
| webapi1-cpnucleo | ghcr.io/jonathanperis/cpnucleo-web-api:latest | 5000 | 5100 |
| webapi2-cpnucleo | ghcr.io/jonathanperis/cpnucleo-web-api:latest | 5000 | 5111 |
| identityapi-cpnucleo | ghcr.io/jonathanperis/cpnucleo-identity-api:latest | 5010 | 5200 |
| grpcserver-cpnucleo | ghcr.io/jonathanperis/cpnucleo-grpc-server:latest | 5020/5021 | 5300/5301 |
| webclient-cpnucleo | ghcr.io/jonathanperis/cpnucleo-web-client:latest | 5030 | 5400 |
| db | postgres:16.7 | 5432 | 5432 |
| nginx | nginx | 9999 | 9999 |

All API services depend on `db` being healthy before starting.

### Development Override (`compose.override.yaml`)

```bash
docker compose -f compose.yaml -f compose.override.yaml up --build
```

Differences from base:

- Builds from source using Dockerfiles in `src/`
- Build args: `AOT=false`, `TRIM=false`, `EXTRA_OPTIMIZE=false`, `BUILD_CONFIGURATION=Debug`
- Adds Grafana LGTM OpenTelemetry stack (ports 3000, 4317, 4318)
- Resource limits: 0.4 CPU, 100MB memory per service

### Production Override (`compose.prod.yaml`)

```bash
docker compose -f compose.yaml -f compose.prod.yaml up -d
```

Differences from base:

- `restart: always` on all services
- Resource reservations: 0.25 CPU / 256MB per API, 0.50 CPU / 512MB per DB
- Resource limits: 0.50 CPU / 512MB per API, 1.0 CPU / 1GB for DB
- JSON logging with rotation: 10MB max size, 3 files retained
- No build step; production image variables such as `CPNUCLEO_WEB_API_IMAGE` are required and should point at immutable GHCR tags (for example `sha-...`)

---

## Dockerfiles

Each service has a multi-stage Dockerfile supporting configurable build options:

### Build Arguments

| Argument | Description | Dev Value | Prod Value |
|----------|-------------|-----------|------------|
| `AOT` | Enable Native AOT compilation | false | false |
| `TRIM` | Enable assembly trimming with ReadyToRun | false | true |
| `EXTRA_OPTIMIZE` | Aggressive optimizations (remove symbols, disable debugger, invariant globalization) | false | true |
| `BUILD_CONFIGURATION` | .NET build configuration | Debug | Release |
| `ASPNETCORE_ENVIRONMENT` | Runtime environment | Development | Production |
| `DB_CONNECTION_STRING` | Database connection string | (from .env) | (from secrets) |

### Build Stages

1. **base** -- `mcr.microsoft.com/dotnet/aspnet:10.0` runtime image
2. **build** -- `mcr.microsoft.com/dotnet/sdk:10.0` with clang/zlib for AOT support; restores, builds
3. **publish** -- Publishes with configured optimizations
4. **final** -- Copies published output to runtime image

### Platform Support

The release workflow builds `linux/amd64` and `linux/arm64/v8` images, then merges them into `latest` and immutable `sha-${GITHUB_SHA}` manifests.

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

---

## GitHub Actions CI/CD

### Build Check (`build-check.yml`)

Triggered on pull requests to main.

**Jobs:**

1. **Setup, Build & Test** (matrix: WebApi, GrpcServer, IdentityApi, WebClient)
   - Checkout repository
   - Setup .NET SDK (from global.json)
   - Restore dependencies
   - Build application (Debug, no AOT/Trim)
   - Run Architecture Tests

2. **Container Healthcheck Test** (depends on build)
   - Build Docker image from source
   - Start container via Docker Compose
   - Poll `/healthz` endpoint up to 20 times with 5-second intervals
   - Fail if health check does not return 200

### Main Release (`main-release.yml`)

Triggered on push to main and manual dispatch.

**Jobs:**

1. **Setup, Build & Test** -- Same as build check but with `TRIM=true`, `EXTRA_OPTIMIZE=true`, `BUILD_CONFIGURATION=Release`

2. **Build & Push Docker Images** (depends on test)
   - Build `linux/amd64` images tagged `sha-${GITHUB_SHA}-amd64` and `latest`
   - Build `linux/arm64/v8` images tagged `sha-${GITHUB_SHA}-arm64` and `latest-arm64`
   - Merge both architectures into multi-arch `sha-${GITHUB_SHA}` and `latest` manifests for each GHCR image

3. **Container Healthcheck Test** (depends on push)
   - Pull production images
   - Run healthcheck validation

4. **Deploy to Azure** (depends on infrastructure + manifest merge)
   - Deploy each service to its Azure Web App
   - Uses Azure OIDC secrets (`AZURE_CLIENT_ID`, `AZURE_TENANT_ID`, `AZURE_SUBSCRIPTION_ID`) and immutable `sha-${GITHUB_SHA}` image tags

### Azure Deployment Targets

| Service | Azure Web App Name | Environment |
|---------|-------------------|-------------|
| WebApi | cpnucleo-api-dotnet | production-webapi |
| GrpcServer | cpnucleo-grpc-server | production-grpcserver |
| IdentityApi | cpnucleo-identity-api | production-identityapi |
| WebClient | cpnucleo-webclient-dotnet | production-webclient |

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
| `DB_CONNECTION_STRING` | Production database connection |
| `AZURE_CLIENT_ID` | Azure OIDC client ID |
| `AZURE_TENANT_ID` | Azure tenant ID |
| `AZURE_SUBSCRIPTION_ID` | Azure subscription ID |

---

## Network

All services communicate over a shared Docker bridge network:

```yaml
networks:
  default:
    name: cpnucleo_network
    driver: bridge
```

Service discovery uses Docker DNS (e.g., `db`, `webapi1-cpnucleo`).
