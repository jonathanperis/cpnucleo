# CPnucleo product context

## Product identity and purpose

CPnucleo is a hands-on .NET 10 project-management learning laboratory. It demonstrates Clean Architecture boundaries, incremental domain modeling, REST and gRPC transports, EF Core and Dapper, PostgreSQL, Astro with native TypeScript, Docker, NGINX, OpenTelemetry, GitHub Actions, GHCR and Hostinger deployment. Its purpose is a dependable sandbox with explicit experiments and tradeoffs.

The public GitHub Pages site under `docs/` is the trust surface for the repository. It should help a technical visitor decide whether the repo is worth cloning, studying, adapting, or using as an architecture reference.

## Register

brand

## Primary users

- Senior .NET engineers evaluating architecture patterns, data access tradeoffs, and service boundaries.
- Backend and platform engineers checking Docker, NGINX, CI/CD, telemetry, and container deployment practice.
- Technical reviewers and recruiters who need current, inspectable evidence and explicit maturity boundaries.
- Maintainers returning to the docs to find commands, topology, tests, API contracts, and deployment notes.

Users usually arrive in evaluation mode. They are skeptical, time constrained, and want proof trails more than marketing claims.

## Core value propositions

- Clean Architecture enforced by automated architecture tests.
- Dual transport implementation: REST through FastEndpoints and gRPC style messaging through FastEndpoints Remote Messaging.
- Persistence comparisons against shared PostgreSQL: EF Core and Dapper examples in REST, Dapper in gRPC.
- Four-service presentation layer: WebApi, GrpcServer, IdentityApi, and WebClient.
- Production path through Docker, NGINX, GHCR, GitHub Actions, Hostinger Docker Manager, and OpenTelemetry.
- Documentation that maps architecture, API reference, database, tests, project structure, technologies, and deployment.

## Canonical facts for copy

Use these facts only when the source remains true in README, AGENTS.md, solution files, or docs:

- Runtime: .NET 10.
- UI: Astro static templates plus native TypeScript and Tailwind CSS, styled with the shared paper-and-ink design system.
- REST: FastEndpoints with EF Core, explicit Dapper and generic Dapper/UoW examples.
- gRPC style messaging: FastEndpoints Remote Messaging with Dapper through GrpcServer.
- Authentication: dedicated IdentityApi as an OpenID Connect provider (authorization code with PKCE, client credentials) with Argon2id-hashed credentials.
- Database: PostgreSQL with Npgsql; exact versions live in Compose/project files.
- Reverse proxy: NGINX with least-connection load balancing.
- Observability: OpenTelemetry with OTLP export and optional Grafana LGTM stack.
- CI/CD: GitHub Actions, GHCR, CodeQL, Hostinger Docker Manager.
- Public docs: `https://jonathanperis.github.io/cpnucleo/`.
- Live demo: canonical Hostinger URL from README and docs navigation.

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

- Generic dark developer landing pages: neon accents, glowing grids, gradient headings, uppercase monospace eyebrows and pill chips.
- SaaS hero pages built around big metrics without proof links.
- Docs pages that concatenate every article into one dense wall.
- Decorative terminal cosplay that hides the real architecture.
- Repetitive icon card grids and side-stripe callouts.

## Aesthetic

A printed technical manual rather than a developer console: warm paper, near-black ink, one vermilion signal, Newsreader headlines with IBM Plex for the interface and code. The docs site and the WebClient share it. `DESIGN.md` holds the palette, type roles and recurring devices (registration-mark brand, numbered sections, ruled ledgers, captioned figures, colophon).

## Current-site maintenance

The home page leads with a headline and a captioned service map, then a ledger of what the lab compares, the contents and a runnable quick start. The documentation index is a contents page with numbered chapters; articles have running heads, an "On this page" margin and previous/next links. Keep the learning-laboratory identity, runnable quick start and source-backed claims consistent with the repository. See `docs/audit-2026-09.md` for the current enhancement plan.

## Merge and review policy

Work on branches and PRs. Do not merge PRs without explicit approval from Jonathan in the current conversation.
