---
name: Cpnucleo Deployment and CI/CD
description: Azure deployment details, GHCR registry, GitHub Actions secrets, Docker compose configs
type: reference
---

## Azure Deployment

Live demo: https://cpnucleo-webclient-dotnet.azurewebsites.net

Four services deployed to Azure Web Apps:
- cpnucleo-api-dotnet (WebApi)
- cpnucleo-grpc-server (GrpcServer)
- cpnucleo-identity-api (IdentityApi)
- cpnucleo-webclient-dotnet (WebClient)

## GitHub Secrets Required

| Secret | Purpose |
|--------|---------|
| `GITHUB_TOKEN` | GHCR auth (automatic) |
| `AZURE_WEBAPP_PUBLISH_PROFILE_WEBAPI` | Azure WebApi deploy |
| `AZURE_WEBAPP_PUBLISH_PROFILE_GRPCSERVER` | Azure GrpcServer deploy |
| `AZURE_WEBAPP_PUBLISH_PROFILE_IDENTITYAPI` | Azure IdentityApi deploy |
| `AZURE_WEBAPP_PUBLISH_PROFILE_WEBCLIENT` | Azure WebClient deploy |

## Docker Compose Configs

- `compose.yaml` — Base: pre-built GHCR images
- `compose.override.yaml` — Dev: build from source + Grafana LGTM (0.4 CPU, 100MB per service)
- `compose.prod.yaml` — Prod: restart policies, resource reservations (0.25-0.50 CPU, 256-512MB), JSON logging

## GHCR Images

- `ghcr.io/jonathanperis/cpnucleo-webapi:latest`
- `ghcr.io/jonathanperis/cpnucleo-grpcserver:latest`
- `ghcr.io/jonathanperis/cpnucleo-identityapi:latest`
- `ghcr.io/jonathanperis/cpnucleo-webclient:latest`

All multi-platform: linux/amd64 + linux/arm64/v8
