# CPnucleo product context

## Product identity and purpose

CPnucleo is a production-grade .NET 10 project management and task tracking reference system. It demonstrates Clean Architecture, Domain-Driven Design, REST and gRPC transports, dual data access with EF Core and Dapper, PostgreSQL persistence, Blazor UI, Docker, NGINX, OpenTelemetry, GitHub Actions, GHCR, and Azure Web Apps deployment.

The public GitHub Pages site under `docs/` is the trust surface for the repository. It should help a technical visitor decide whether the repo is worth cloning, studying, adapting, or using as an architecture reference.

## Register

brand

## Primary users

- Senior .NET engineers evaluating architecture patterns, data access tradeoffs, and service boundaries.
- Backend and platform engineers checking Docker, NGINX, CI/CD, telemetry, and Azure deployment practice.
- Technical reviewers and recruiters who need proof that the project is complete, current, and inspectable.
- Maintainers returning to the docs to find commands, topology, tests, API contracts, and deployment notes.

Users usually arrive in evaluation mode. They are skeptical, time constrained, and want proof trails more than marketing claims.

## Core value propositions

- Clean Architecture enforced by automated architecture tests.
- Dual transport implementation: REST through FastEndpoints and gRPC style messaging through FastEndpoints Remote Messaging.
- Dual persistence strategies against the same PostgreSQL database: EF Core for REST paths, Dapper and Unit of Work for gRPC paths.
- Four-service presentation layer: WebApi, GrpcServer, IdentityApi, and WebClient.
- Production path through Docker, NGINX, GHCR, GitHub Actions, Azure Web Apps, and OpenTelemetry.
- Documentation that maps architecture, API reference, database, tests, project structure, technologies, and deployment.

## Canonical facts for copy

Use these facts only when the source remains true in README, AGENTS.md, solution files, or docs:

- Runtime: .NET 10.
- UI: Blazor Server plus WebAssembly with MudBlazor components.
- REST: FastEndpoints with EF Core through WebApi.
- gRPC style messaging: FastEndpoints Remote Messaging with Dapper through GrpcServer.
- Authentication: dedicated IdentityApi with JWT and PBKDF2-hashed credentials.
- Database: PostgreSQL 16.7 with Npgsql.
- Reverse proxy: NGINX with least-connection load balancing.
- Observability: OpenTelemetry with OTLP export and optional Grafana LGTM stack.
- CI/CD: GitHub Actions, GHCR, CodeQL, Azure Web Apps.
- Public docs: `https://jonathanperis.github.io/cpnucleo/`.
- Live demo: Azure WebClient URL from README and docs navigation.

Avoid unsupported claims like full coverage unless the current test reports prove them.

## Brand voice

Voice words: architectural, inspectable, precise, restrained, systems-minded.

Copy principles:

- Lead with proof, not hype.
- Name the source artifact behind each claim when possible.
- Prefer concrete nouns: architecture tests, REST endpoints, gRPC handlers, compose topology, release workflow.
- Keep availability, status, and route labels direct.
- Avoid inflated enterprise language unless it is tied to implementation evidence.

## Anti-references

- Generic cyberpunk developer landing pages with neon particles, glowing grids, and gradient headings.
- SaaS hero pages built around big metrics without proof links.
- Docs pages that concatenate every article into one dense wall.
- Decorative terminal cosplay that hides the real architecture.
- Repetitive icon card grids and side-stripe callouts.

## Current aesthetic to preserve

- Dark technical environment.
- Cyan as the primary system accent.
- Thin-line architecture diagrams.
- Code and command panels.
- Compact proof chips.
- A sense of an engineering console or architecture workbench.

## Current-site opportunities

- Shift from cyber template cues to architecture workbench cues.
- Make proof inspectable above the fold.
- Put Documentation before Live Demo on the GitHub Pages surface.
- Turn `/docs/` into a command center with links to individual pages.
- Improve docs readability, active navigation state, and empty search feedback.
- Remove the accidental Astro route emitted by helper files under `src/pages`.

## A/B testing hypotheses

- Hero proof density: a proof-led hero should increase docs and GitHub clicks compared with a broad marketing hero.
- CTA hierarchy: docs-first ordering should better match GitHub Pages visitor intent than demo-first ordering.
- Docs root model: a command center should improve orientation compared with one long concatenated page.
- Visual noise level: a quieter console aesthetic should improve credibility without losing technical identity.

## Merge and review policy

Work on branches and PRs. Do not merge PRs without explicit approval from Jonathan in the current conversation.
