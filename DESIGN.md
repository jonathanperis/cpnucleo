# CPnucleo design context

## Design intent

The CPnucleo GitHub Pages site is a technical brand and documentation surface. It should feel like an architecture workbench: dark, precise, inspectable, and source-backed. The design is allowed to be expressive, but proof and readability come before spectacle.

## Physical scene sentence

A senior .NET engineer is reviewing the repository late in the evening on a desktop monitor, deciding whether to clone it, study its service boundaries, or trust its deployment practices.

Dark theme is correct because the scene is focused technical evaluation in a low-light work environment. The tone should be calm and exact, not nightclub neon.

## Visual anchors

- Architecture console.
- Release notebook.
- Service topology map.
- Build pipeline ledger.
- Astro, Qwik, and Tailwind CSS on the live app side, with Catalyst-inspired product UI patterns implemented as resumable Qwik components while the Pages site remains a static docs and proof surface.

## Color strategy

Restrained dark technical system with one cyan accent and one secondary blue-violet status role.

Use OKLCH tokens for new work:

```css
:root {
  --ink-950: oklch(13% 0.025 255);
  --ink-900: oklch(17% 0.03 255);
  --ink-850: oklch(21% 0.035 255);
  --ink-800: oklch(25% 0.04 255);
  --line: oklch(42% 0.06 250 / 0.42);
  --line-strong: oklch(62% 0.12 225 / 0.58);
  --text-main: oklch(94% 0.018 245);
  --text-body: oklch(82% 0.028 245);
  --text-muted: oklch(66% 0.035 245);
  --cyan: oklch(78% 0.17 215);
  --cyan-soft: oklch(78% 0.17 215 / 0.14);
  --violet: oklch(70% 0.14 285);
  --success: oklch(73% 0.14 160);
}
```

Rules:

- No pure black or pure white.
- No gradient text.
- Cyan should mark interaction, proof, and diagram edges, not decorate every surface.
- Background grids can remain subtle, under 6 percent visual weight.
- Remove floating particles and decorative glow fields unless they communicate system state.

## Typography

Use a readable sans for body and navigation. Use mono only for code, commands, compact labels, and technical IDs.

Recommended roles:

- Body and UI: `Atkinson Hyperlegible`, system sans fallback.
- Display and section headings: `Rajdhani`, used sparingly for the product name, hero, and proof labels.
- Code and terminal blocks: `JetBrains Mono`.

Hierarchy:

- Hero h1: fluid clamp, strong weight, solid color.
- Body: 65 to 75 character line length where possible.
- Docs body: larger and calmer than the current all-mono setting.
- Labels: uppercase only for short system labels, not paragraph copy.

## Layout principles

- Home is a proof path, not a generic landing page.
- Default route order: hero, proof ledger, architecture map, docs paths, quick start.
- Use asymmetry through proof columns and diagrams, not random card grids.
- Prefer ledgers, rows, maps, and command panels over repeated feature cards.
- Docs root should be a command center linking to individual pages.
- Individual docs pages should support long-form reading, tables, code, and sidebar navigation.

## Component direction

### Hero

- Headline: `Clean Architecture reference system for .NET 10`.
- Subcopy should name REST, gRPC, Qwik, PostgreSQL, Docker, Hostinger, and tests without overclaiming.
- CTA hierarchy: Documentation, GitHub, Live Demo.
- Include visible A/B variant controls when testing.

### Proof ledger

Use compact evidence rows instead of standalone metric cards. Good rows:

- REST and gRPC parity.
- Architecture tests.
- Service topology.
- CI/CD and deployment path.

### Architecture diagram

Use service boxes and connecting lines. Keep it semantic and readable. Avoid decorative pseudo-metrics.

### Docs cards

Use grouped documentation paths with descriptions and route links. Avoid identical icon-card grids.

### Code panels

Use tinted dark panels, clear labels, and horizontal scroll. No pure black terminal blocks.

### Blockquotes and callouts

No side-stripe accents. Use full borders, subtle background tint, or inline labels.

## Motion

- Motion should be brief and purposeful.
- Animate opacity and transform, not width, height, padding, or margin.
- Respect `prefers-reduced-motion`.
- Avoid perpetual particles and ambient drift for this site.

## Accessibility standards

- Keyboard-visible focus on all links, buttons, search fields, and menu controls.
- Body and docs text must meet WCAG AA contrast on dark backgrounds.
- Mobile sidebar must expose `aria-expanded` state and close predictably.
- Search must provide an empty-result message.
- Code and tables must support horizontal overflow without layout breakage.

## Copy standards

- No em dashes in new interface copy.
- No generic filler like production-ready without nearby proof.
- Use exact artifact names: `Architecture.Tests`, `compose.yaml`, `main-release.yml`, `WebApi`, `GrpcServer`, `IdentityApi`, `WebClient`.
- Avoid claiming complete test coverage unless verified by reports.
- Prefer action labels: Read docs, Inspect GitHub, Open live demo.

## Known issues to address

- `docs/src/pages/docs/sidebar.config.ts` must not live under `src/pages` because Astro emits it as `/docs/sidebar.config`.
- Existing detector findings include gradient text, pure black code background, layout width transition, and side-tab callout styling.
- The docs root currently behaves like a long compiled page rather than an index.
- Sidebar and docs typography are too mono-heavy for sustained reading.

## Quality bar

Before a Pages design PR is considered ready:

- Root `PRODUCT.md` and `DESIGN.md` load through the Impeccable context loader.
- `git diff --check` passes.
- `bun run build` passes from `docs/`.
- Impeccable detector has no new high-signal UI anti-patterns.
- Browser smoke covers home, docs root, one individual docs page, search, mobile navigation, and console errors.
- Temporary preview servers are stopped.
- PR is opened or updated, but not merged without explicit approval.
