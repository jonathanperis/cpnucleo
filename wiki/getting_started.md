# Getting Started

## Prerequisites

| Tool | Version | Notes |
|------|---------|-------|
| .NET SDK | 10.0.102+ | Specified in `global.json` with `latestMinor` roll-forward |
| Docker | Latest | For running with Docker Compose |
| Docker Compose | v2+ | Bundled with Docker Desktop |
| PostgreSQL | 16.7 | Provided via Docker; only needed if running locally without Docker |

---

## Clone the Repository

```bash
git clone https://github.com/jonathanperis/cpnucleo.git
cd cpnucleo
```

---

## Run with Docker Compose (Recommended)

### Default Mode (Pre-built Images)

Uses pre-built images from GHCR:

```bash
docker compose up
```

### Development Mode

Builds from source with debug configuration and includes an OpenTelemetry/Grafana LGTM stack for observability:

```bash
docker compose -f compose.yaml -f compose.override.yaml up --build
```

### Production Mode

Uses pre-built images with resource reservations, restart policies, and structured JSON logging:

```bash
docker compose -f compose.yaml -f compose.prod.yaml up -d
```

### Services and Ports

Once running, the services are available at:

| Service | URL | Description |
|---------|-----|-------------|
| WebApi (instance 1) | http://localhost:5100 | REST API |
| WebApi (instance 2) | http://localhost:5111 | REST API (load-balanced pair) |
| IdentityApi | http://localhost:5200 | JWT Authentication API |
| GrpcServer | http://localhost:5300 (health) / :5301 (gRPC) | gRPC command server |
| WebClient | http://localhost:5400 | Blazor UI |
| NGINX | http://localhost:9999 | Reverse proxy (load balances WebApi) |
| PostgreSQL | localhost:5432 | Database |
| Grafana (dev only) | http://localhost:3000 | Observability dashboard |
| OTLP Collector (dev only) | localhost:4317 (gRPC) / :4318 (HTTP) | OpenTelemetry collector |

### Health Checks

All services expose a health endpoint:

```bash
curl http://localhost:5100/healthz   # WebApi
curl http://localhost:5200/healthz   # IdentityApi
curl http://localhost:5300/healthz   # GrpcServer
curl http://localhost:5400/healthz   # WebClient
```

---

## Run Locally (Without Docker)

### 1. Start PostgreSQL

Ensure PostgreSQL 16+ is running locally. Execute the init scripts to set up the schema:

```bash
psql -U postgres -f docker-entrypoint-initdb.d/001-track-commit-timestamp.sql
psql -U postgres -d cpnucleo -f docker-entrypoint-initdb.d/002-database-dump-ddl.sql
```

### 2. Configure Environment

The project uses environment variables loaded from the `.env` file. For local development, update the connection string to point to localhost:

```env
POSTGRES_USER=postgres
POSTGRES_PASSWORD=postgres
POSTGRES_DB=cpnucleo
DB_CONNECTION_STRING=Host=localhost;Username=postgres;Password=postgres;Database=cpnucleo;Minimum Pool Size=10;Maximum Pool Size=10;Multiplexing=true
OTEL_EXPORTER_OTLP_ENDPOINT=http://localhost:4317
OTEL_METRIC_EXPORT_INTERVAL=5000
```

### 3. Build the Solution

```bash
dotnet build cpnucleo.slnx
```

### 4. Run the Services

Each service must be run in a separate terminal:

```bash
# Terminal 1 - REST API
cd src/WebApi && dotnet run

# Terminal 2 - Identity/Auth API
cd src/IdentityApi && dotnet run

# Terminal 3 - gRPC Server
cd src/GrpcServer && dotnet run

# Terminal 4 - Blazor Web Client
cd src/WebClient && dotnet run
```

---

## Swagger UI

When running in Development mode, Swagger UI is available for interactive API exploration:

- **WebApi**: http://localhost:5100/swagger
- **IdentityApi**: http://localhost:5200/swagger

---

## Generate Fake Data

Set `CreateFakeData=true` in the application configuration to generate CSV/SQL dump files using Bogus. The generated files should be placed in `docker-entrypoint-initdb.d/` for automatic database seeding on container startup.
