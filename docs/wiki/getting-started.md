# Getting Started

## Minimal learning stack

Docker Compose v2 is sufficient to build and start the app:

```sh
docker compose -f compose.lab.yaml up --build -d
docker compose -f compose.lab.yaml run --rm seed
```

Open http://localhost:5400. The local account is `demo@cpnucleo.local` / `LocalLearning@123`. The lab is bound to loopback interfaces and uses disposable example credentials. Production has independently configured credentials and no automatic demo seeding.

The minimal stack contains PostgreSQL, a one-shot migrator, REST, Identity and the Astro frontend. Add `--profile full` for gRPC and `--profile observability` for Grafana LGTM. The full legacy load-balanced topology remains available through `compose.yaml` plus `compose.override.yaml`; copy `.env.example` to `.env` before using it.

| Service | Local address |
|---|---|
| WebClient | http://localhost:5400 |
| REST | http://localhost:5100 |
| Identity | http://localhost:5200 |
| gRPC | http://localhost:5300 (HTTP/2) |
| gRPC health | http://localhost:5301/healthz |
| PostgreSQL | localhost:15432 |
| Grafana | http://localhost:3000 |

## Data profiles

The seed command requires an empty database. The default `tiny` profile creates 3 projects and 30 tasks. To replace **disposable local data** with 50 projects and 500 tasks:

```sh
docker compose -f compose.lab.yaml run --rm seed --reset-lab --Seed:Profile=realistic
```

`--reset-lab` drops the configured database and is accepted only in Development. Do not point Development configuration at valuable data. The large CSV importer is a separate advanced exercise and remains explicitly opt-in.

## Source development

Install .NET 10 SDK (see `global.json`), Node.js 22.14+ and Bun 1.3.11+. Set `DB_CONNECTION_STRING`, a development `Jwt__SigningKey`, and matching CORS origins in the service environment or .NET user secrets. `dotnet run` does not automatically import the repository dotenv file.

```sh
dotnet build cpnucleo.slnx
dotnet run --project src/WebApi
```

Run the other services in separate terminals. From `src/WebClient`, use `bun install --frozen-lockfile` and `bun run dev`. Browser service URLs default to localhost; deployment supplies explicit public build-time URLs.

## Readiness and tests

```sh
curl --fail http://localhost:5100/healthz
curl --fail http://localhost:5100/readyz
dotnet test cpnucleo.slnx
```

The database integration suite provisions a separate Testcontainers PostgreSQL instance. Continue with [Learning Lab](learning-lab) for guided exercises.

## Production

Use `compose.prod.yaml` alone with the variables documented in `.env.hostinger.example`. See [Deployment](deployment). Do not combine production with development files.
