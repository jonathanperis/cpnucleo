# CPnucleo GitHub Pages design overhaul plan

Target surface: `docs/` Astro GitHub Pages site at `https://jonathanperis.github.io/cpnucleo/`.

Source synced before review: `main` from `origin/main`, reviewed on 2026-05-16.

## Current design health

Overall score: 25 / 40.

| Area | Score | Finding |
| --- | ---: | --- |
| System status | 3 | CTAs and docs navigation are visible, but active doc state is subtle. |
| Match with user expectations | 3 | Technical positioning is clear for .NET developers. |
| User control | 2 | Docs root loads every article as one long page. Search filters sections, but page context is heavy. |
| Consistency | 3 | Home and docs share a coherent dark cyan system. |
| Error prevention | 2 | Root docs route, per-page docs routes, and hash-only sidebar links create unclear navigation intent. |
| Recognition over recall | 3 | Stack chips and sidebar labels help scanning. |
| Efficiency | 2 | Documentation scanning is slowed by dense long-form layout and small sidebar text. |
| Aesthetic minimalism | 2 | Neon grid, particles, gradients, glow, metric cards, and identical feature cards create template noise. |
| Error recovery | 2 | Search has no empty-result explanation. Mobile menu state needs validation. |
| Help and docs | 3 | Content volume is strong, but structure needs stronger wayfinding. |

## Impeccable detector findings

`npx impeccable detect --json docs/src/pages/index.astro docs/src/components/home docs/src/layouts docs/src/styles`

1. `gradient-text` in `docs/src/styles/globals.css:253`.
2. `side-tab` in `docs/src/styles/docs.css:313`.
3. `pure-black-white` in `docs/src/styles/globals.css:561`.
4. `layout-transition` in `docs/src/styles/globals.css:198`.
5. `single-font` warning from `docs/src/layouts/BaseLayout.astro`, likely because docs use one dominant code-style family.

Build also reports an Astro route smell:

- `docs/src/pages/docs/sidebar.config.ts` is emitted as `/docs/sidebar.config`.
- Move it to `docs/src/lib/sidebar.config.ts` or another non-route folder before implementation.

## What works now

- The site immediately communicates a serious .NET architecture reference project.
- The dark cyan GitHub Pages aesthetic is consistent between home and docs.
- The stack chips, quick start, and docs sidebar make the project easy to classify.
- The architecture diagram motif fits the repository better than generic product screenshots.

## Main problems to fix

### P1: It reads like a generated cyber landing page

Evidence: neon grid, cyan glow, gradient heading, monospace everything, uppercase labels, floating particles, metric cards, identical feature cards.

Fix: keep the dark technical aesthetic, but make it feel like an architecture workbench rather than a cyber template. Use fewer effects, stronger information design, source-backed proof, and artifact previews.

### P1: The proof is broad, not inspectable

The page claims 55 REST endpoints, 55 gRPC handlers, architecture tests, CI/CD, Azure, and Docker. Those are valuable, but visitors do not get fast proof trails.

Fix: turn generic stats into source-backed proof modules:

- REST and gRPC parity panel with links to API docs.
- Architecture gate panel with NetArchTest count and CI link.
- Runtime topology panel with WebApi, IdentityApi, GrpcServer, WebClient, PostgreSQL, and NGINX.
- Quick start panel that separates local, Docker, and production docs.

### P1: Docs root is too dense

`/docs/` renders all wiki sections into one long page. That helps search, but hurts orientation and performance of understanding.

Fix: keep root as a docs command center, not a full concatenation. Link to individual pages, show high-value summaries, add a strong per-page reading layout, and keep search focused.

### P2: Documentation ergonomics need polishing

Sidebar type is small, active state is subtle, search has no visible empty state, and content rhythm is dense.

Fix: increase docs body readability, make active section unmistakable, add an empty-search message, strengthen table/code block affordances, and smoke mobile drawer behavior.

### P2: Implementation hygiene issue in Astro route tree

`sidebar.config.ts` under `src/pages` becomes a generated route.

Fix: move config to `src/lib/sidebar.config.ts` and update imports.

## Same-aesthetic overhaul direction

Scene sentence: a senior .NET engineer is reviewing this repo at night on a desktop monitor before deciding whether to clone it, use it as architecture reference, or inspect deployment practices.

Theme: dark remains correct, but the surface should feel like an architectural console and release notebook, not neon cyberpunk.

Color strategy: restrained dark technical system with one cyan accent and one secondary steel or violet accent for status grouping. Convert raw hex colors to OKLCH tokens.

Typography: keep a technical voice, but stop using one monospace family for all reading. Use a readable sans for body and a mono only for code, labels, commands, and diagrams.

Layout: replace template cards with three source-backed proof zones:

1. Architecture map.
2. Endpoint and transport parity.
3. Build, test, deploy pipeline.

## Proposed information architecture

### Home page

1. Hero: `Clean Architecture reference system for .NET 10`.
2. Short proof line: REST, gRPC, Blazor, PostgreSQL, NGINX, Azure, architecture tests.
3. CTA order: Docs, GitHub, Live Demo.
4. Architecture map: four service boxes plus infrastructure layer and domain core.
5. Proof ledger: endpoint parity, architecture tests, CI/CD, Docker topology.
6. Quick start: clone, run, inspect docs.
7. Explore paths: architecture, API reference, testing, deployment.

### Docs root

1. Documentation index with grouped entry points.
2. Search with empty-state feedback.
3. Project map table.
4. Recently important proof links: architecture rules, API reference, deployment pipeline.
5. No full concatenation of all article content on root unless explicitly wanted.

### Individual docs pages

1. Strong page title and one-sentence purpose.
2. Local table of contents when headings are long.
3. Wider readable content column.
4. Code and tables with clearer scroll affordances.
5. Sidebar active state and mobile drawer verification.

## A/B testing plan

### Test 1: Hero proof density

Hypothesis: developers will click deeper when the first fold gives concrete architecture proof instead of broad marketing language.

A/control: current hero with headline, stack chips, and three CTAs.

B, proof ledger: hero plus four compact proof rows: endpoint parity, architecture tests, service topology, deploy target.

C, architecture-first: hero text plus large architecture map as the dominant visual, with CTAs below the map.

Primary metric: docs CTA click-through and GitHub click-through.

Recommendation: implement B first. It preserves the current aesthetic and changes proof density without destabilizing layout.

### Test 2: CTA hierarchy

Hypothesis: this Pages site is more valuable as documentation and architecture evidence than as a live demo launcher.

A/control: Try it Live, Documentation, GitHub.

B, docs-first: Documentation, GitHub, Live Demo.

C, clone-first: GitHub, Documentation, Live Demo, with quick-start command immediately visible.

Primary metric: CTA click distribution and bounce from home.

Recommendation: implement B first for GitHub Pages. The page itself is the docs gateway.

### Test 3: Docs root model

Hypothesis: a docs command center will improve orientation compared with a single page containing every article.

A/control: root renders every section in one long scroll.

B, command center: root shows grouped summaries and links to individual docs pages.

C, hybrid: root shows summaries plus expandable previews for each section.

Primary metric: sidebar clicks, search use, docs page depth, and time to first docs-page click.

Recommendation: implement B first. It reduces cognitive load and uses routes that already exist.

### Test 4: Visual noise level

Hypothesis: reducing cyber effects while keeping dark cyan structure will make the project feel more credible.

A/control: grid, particles, glow, gradient headline, metric cards.

B, quiet console: keep dark grid and cyan accent, remove particles and gradient text, use solid headings and proof panels.

C, blueprint notebook: lighter dark surface, thin line diagrams, fewer glows, more table-led proof.

Primary metric: scroll depth and docs CTA click-through.

Recommendation: implement B first. It is the safest same-aesthetic upgrade.

## Implementation phases

### Phase 1: Route and docs hygiene

- Move `docs/src/pages/docs/sidebar.config.ts` to `docs/src/lib/sidebar.config.ts`.
- Change docs root so it is an index instead of a full concatenation, or add a feature flag for A/B testing root models.
- Add search empty state.
- Run `bun run build` and verify `/cpnucleo/docs/` plus individual pages.

### Phase 2: Home proof redesign

- Rewrite hero copy around architecture reference value.
- Replace metric cards with proof ledger and architecture map.
- Remove gradient text, side-tab accents, pure black, and layout-property transitions.
- Keep cyan, grid, and technical atmosphere.

### Phase 3: A/B test plumbing

- Add a query parameter or local-storage variant selector, for example `?ab=control`, `?ab=proof`, `?ab=architecture`.
- Emit lightweight click events through existing analytics component if available.
- Keep all variants same-aesthetic and source-backed.

### Phase 4: Verification

- `git diff --check`.
- `bun run build` from `docs/`.
- `npx impeccable detect --json docs/src/pages/index.astro docs/src/components docs/src/layouts docs/src/styles`.
- Local preview with base-aware route checks.
- Browser smoke home, docs root, one long docs page, one code-heavy docs page, search, mobile drawer, and console.

## Files likely to change

- `docs/src/pages/index.astro`
- `docs/src/components/home/Hero.astro`
- `docs/src/components/home/Dashboard.astro`
- `docs/src/components/home/Navbar.astro`
- `docs/src/pages/docs/[...slug].astro`
- `docs/src/pages/docs/sidebar.config.ts`, moved to `docs/src/lib/sidebar.config.ts`
- `docs/src/styles/globals.css`
- `docs/src/styles/docs.css`

## Merge policy

Open PRs only. Do not merge without explicit approval from Jonathan in the current conversation.
