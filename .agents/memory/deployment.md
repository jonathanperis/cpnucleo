---
name: Cpnucleo Deployment and CI/CD
description: Hostinger Docker Manager deployment details, GHCR registry, GitHub Actions secrets, Docker compose configs
type: reference
---

## Hostinger Deployment

Live demo: https://cpnucleo.jonathanperis.tech/

Production is deployed through Hostinger Docker Manager with the repository production Compose configuration and immutable amd64 GHCR images.

Public surfaces:
- WebClient: https://cpnucleo.jonathanperis.tech/
- WebApi: https://api-cpnucleo.jonathanperis.tech/
- IdentityApi: https://identity-cpnucleo.jonathanperis.tech/
- gRPC health: https://grpc-cpnucleo.jonathanperis.tech/healthz

## GitHub Secrets Required

| Secret | Purpose |
|--------|---------|
| `GITHUB_TOKEN` | GHCR auth (automatic) |
| `HOSTINGER_API_TOKEN` | Hostinger API authentication |
| `HOSTINGER_VPS_ID` | Target VPS identifier |
| `HOSTINGER_PROJECT_NAME` | Docker Manager project name |
| `HOSTINGER_ENV_BASE64` | Base64 encoded production environment payload |
| `CPNUCLEO_WEB_URL` | WebClient smoke-test URL |
| `CPNUCLEO_API_URL` | WebApi smoke-test URL |
| `CPNUCLEO_IDENTITY_URL` | IdentityApi smoke-test URL |
| `CPNUCLEO_GRPC_HEALTH_URL` | gRPC health smoke-test URL |

## Docker Compose Configs

- `compose.yaml` — Base: pre-built GHCR images
- `compose.lab.yaml` — Recommended minimal learning stack, explicit migration/seed and optional gRPC/observability
- `compose.override.yaml` — Legacy dev: source builds and published Grafana/Collector ports; application limits differ from infrastructure
- `compose.prod.yaml` — Prod: restart policies, resource reservations (0.25-0.50 CPU, 256-512MB), JSON logging

## GHCR Images

- `ghcr.io/jonathanperis/cpnucleo-web-api:latest`
- `ghcr.io/jonathanperis/cpnucleo-grpc-server:latest`
- `ghcr.io/jonathanperis/cpnucleo-identity-api:latest`
- `ghcr.io/jonathanperis/cpnucleo-web-client:latest`

Main release publishes `sha-<commit>`, `sha-<commit>-amd64`, `sha-<commit>-arm64`, and `latest-arm64` tags. Hostinger uses standalone `compose.prod.yaml`, applies additive migrations and deploys the tested `sha-<commit>-amd64` tags without waiting for the slower arm64/manifest lane. Multi-arch manifests wait for both builds and amd64 container checks; arm64 runtime testing is not implied.
