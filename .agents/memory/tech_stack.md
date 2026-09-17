---
name: Cpnucleo Technology Versions
description: Pinned versions for .NET SDK, NuGet packages, and infrastructure components
type: reference
---

## Authoritative versions

- SDK: `global.json`; Node: `.nvmrc`; Bun: frontend `packageManager` fields and CI/Dockerfiles.
- Backend: project `PackageReference` entries. FastEndpoints packages must remain aligned; EF/Npgsql versions must remain compatible.
- Frontends: `docs/package.json`, `src/WebClient/package.json` and their independent Bun lockfiles. Both use Astro and Tailwind's Vite integration.
- Containers: Compose files and Dockerfiles. PostgreSQL stays on the newest compatible 16.x patch until a separate major data migration is planned.
- See `docs/audit-2026-09.md` for update evidence and compatibility limits. Do not duplicate mutable patch-version inventories here.

## Test Frameworks

- **Architecture Tests:** xUnit + NetArchTest.Rules + FluentAssertions
- **Unit Tests:** NUnit + FakeItEasy + Shouldly
- **Integration Tests:** xUnit v3's `mtp-off` package with VSTest, HTTP/gRPC test hosts and Testcontainers.PostgreSql

## Legacy development service ports

| Service | Internal | External |
|---------|----------|----------|
| WebApi 1 | 5000 | 5100 |
| WebApi 2 | 5000 | 5111 |
| IdentityApi | 5010 | 5200 |
| GrpcServer (gRPC) | 5020 | 5300 |
| GrpcServer (health) | 5021 | 5301 |
| WebClient | 5030 | 5400 |
| NGINX | 9999 | 9999 |
| PostgreSQL | 5432 | 5432 |
| Grafana LGTM (dev) | 3000 | 3000 |

The recommended `compose.lab.yaml` has one REST instance, no NGINX dependency, optional gRPC/observability profiles, and publishes PostgreSQL on loopback port 15432. Production publishes no service/database host ports.
