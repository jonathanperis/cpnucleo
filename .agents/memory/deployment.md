---
name: Cpnucleo Deployment and CI/CD
description: Azure deployment details, GHCR registry, GitHub Actions secrets, Docker compose configs
type: reference
---

## Azure Deployment

Live demo: https://cpnucleo-webclient-dotnet-d6gve6cabpefbmfz.brazilsouth-01.azurewebsites.net/

Four services deployed to Azure Web Apps:
- cpnucleo-api-dotnet (WebApi)
- cpnucleo-grpc-server (GrpcServer)
- cpnucleo-identity-api (IdentityApi)
- cpnucleo-webclient-dotnet (WebClient)

## GitHub Secrets Required

| Secret | Purpose |
|--------|---------|
| `GITHUB_TOKEN` | GHCR auth (automatic) |
| `AZURE_CLIENT_ID` | Azure OIDC app registration client ID |
| `AZURE_TENANT_ID` | Azure tenant ID |
| `AZURE_SUBSCRIPTION_ID` | Azure subscription ID |

## Docker Compose Configs

- `compose.yaml` — Base: pre-built GHCR images
- `compose.override.yaml` — Dev: build from source + Grafana LGTM (0.4 CPU, 100MB per service)
- `compose.prod.yaml` — Prod: restart policies, resource reservations (0.25-0.50 CPU, 256-512MB), JSON logging

## GHCR Images

- `ghcr.io/jonathanperis/cpnucleo-web-api:latest`
- `ghcr.io/jonathanperis/cpnucleo-grpc-server:latest`
- `ghcr.io/jonathanperis/cpnucleo-identity-api:latest`
- `ghcr.io/jonathanperis/cpnucleo-web-client:latest`

Main release also publishes `sha-<commit>`, `sha-<commit>-amd64`, `sha-<commit>-arm64`, and `latest-arm64` tags; the final Azure deploy uses the `sha-<commit>` multi-arch image.
