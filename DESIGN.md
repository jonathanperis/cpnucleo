# Cpnucleo design context

## Design intent

Cpnucleo looks like a well-made technical manual: warm paper, near-black ink, one vermilion signal colour and a serif voice. The same system covers the GitHub Pages site (`docs/`) and the WebClient (`src/WebClient/`), so moving from the documentation into the running app feels like turning a page, not changing products.

The previous dark cyan "architecture console" look was retired in October 2026. It read as generic and machine-made: glowing grids, uppercase monospace eyebrows, pill chips, gradient panels and A/B controls exposed to visitors.

## Physical scene

An engineer reads the docs on a laptop in daylight, sometimes late at night, deciding whether to clone the repository or study one boundary. The page should feel calm, exact and edited by a person. Paper is the default; night is a warm charcoal version of the same page, not a separate neon theme.

## Palette

Paper, ink and vermilion. Values are OKLCH; the WebClient stores them as channel triplets (`--canvas: 97.2% 0.012 85`) so Tailwind utilities can apply alpha.

| Role | Paper (default) | Night | Docs token | WebClient token |
|---|---|---|---|---|
| Page | `oklch(97.2% 0.012 85)` | `oklch(19% 0.012 60)` | `--paper` | `--canvas` |
| Sheet (cards, inputs) | `oklch(98.8% 0.006 85)` | `oklch(22.5% 0.013 60)` | `--sheet` | `--surface` |
| Sunken (rails, code) | `oklch(94.6% 0.016 82)` | `oklch(16.5% 0.011 60)` docs, `oklch(26% 0.014 60)` app | `--paper-sunk` | `--raised` |
| Ink | `oklch(23% 0.02 55)` | `oklch(93.5% 0.016 85)` | `--ink` | `--ink` |
| Body / muted | `oklch(36–50% 0.02 60)` | `oklch(68–83% 0.016 80)` | `--ink-body`, `--ink-muted` | `--muted`, `--subtle` |
| Rules | `oklch(86–88% 0.02 78)` | `oklch(31–32% 0.012 60)` | `--rule` | `--line` |
| Signal | `oklch(58% 0.19 34)` | `oklch(68% 0.17 40)` | `--signal` | `--accent` |
| Signal text | `oklch(50% 0.17 33)` | `oklch(75% 0.14 45)` | `--signal-text` | `--accent-text` |
| Primary action | ink fill, paper text | paper fill, charcoal text | `--button` | `--action` |

Rules:

- Vermilion is a signal, not a fill. It marks the brand nucleus, focus rings, the current place (active chapter, today, current page), section numbers and the one italic phrase in a headline. Primary buttons are inked.
- Errors use a crimson (`--danger`, hue 14) distinct from the vermilion, always with text, never colour alone.
- No gradients, glows, background grids, blurred backdrops or decorative particles.
- Shadows are almost absent: a 1–2px lift on panels, a soft drop under dialogs and menus.
- Radii stay near square: 2–4px for controls and panels, 6px for dialogs. Only avatars are round.

## Typography

| Role | Face | Use |
|---|---|---|
| Display | Newsreader (variable, optical sizes, italic) | Headlines, page titles, section names, running heads, figures and stat numbers |
| Interface and body | IBM Plex Sans (variable) | Paragraphs, navigation, forms, tables |
| Code | IBM Plex Mono 400/500 | Code, commands, identifiers, chapter numbers |

All three are self-hosted through Fontsource (the WebClient CSP allows fonts only from `'self'`).

- Headlines are light (weight 370–420), tight (`letter-spacing` around -0.02em) and solid colour. One italic phrase may carry the signal colour.
- Section labels are serif italic ("What it compares"), paired with a small mono number in vermilion ("§1", "1.2", "03"). Do not reintroduce uppercase tracked monospace eyebrows.
- Body copy stays within 60–72 characters per line.

## Recurring devices

- **Masthead and double rule.** The wordmark and navigation sit above a 3px double rule, then a thin italic folio line.
- **Registration mark.** The brand is a printer's registration mark around a vermilion nucleus (`docs/src/components/Mark.astro`, `src/WebClient/src/components/BrandMark.astro`). Change both together.
- **Numbered sections and chapters.** `§1`, `1.1`, `01`, Part I/II/III. Chapter numbers come from `chapterNumber()` in `docs/src/lib/sidebar.config.ts`.
- **Ledgers and contents.** Rows separated by hairlines under a solid ink rule; the contents uses dot leaders. Prefer these to card grids.
- **Figures with captions.** Diagrams are drawn as thin-line boxes with orthogonal edges and a "Fig. 1." caption. Code samples on the home page are captioned listings.
- **Running heads.** Docs pages open with "Part II, Overview" and "Chapter 03 of 11" in italic.
- **Colophon.** The site ends with a short colophon naming the author, licence and typefaces.

## Layout

- Home: masthead, hero (headline, lede, actions, Fig. 1), §1 ledger of what the lab compares, §2 contents, §3 run it, colophon.
- Docs: a sunken rail with Parts and numbered chapters and a filter; a reading column with running head, article and previous/next links; an "On this page" margin from 1280px.
- WebClient: the same sunken rail for workspace navigation, serif page titles, italic section labels, ruled panels and inked primary actions. Dense data stays in Plex Sans.

## Motion

Brief opacity and transform transitions (160–220ms). No perpetual animation besides loading skeletons. Everything is disabled under `prefers-reduced-motion`.

## Accessibility

- Visible focus on every interactive element (2px vermilion outline).
- Body text meets WCAG AA on both paper and night. Use `--signal-text` / `--accent-text`, not the raw signal, for text.
- The docs drawer exposes `aria-expanded`, closes on Escape and overlay click; the filter announces empty results.
- Code blocks and tables scroll horizontally without breaking the layout.

## Copy

- Plain, specific and short. Name the artifact behind a claim (`Architecture.Tests`, `compose.lab.yaml`, `main-release.yml`, `WebApi`, `GrpcServer`, `IdentityApi`, `WebClient`).
- No em dashes in interface copy. No "production-ready", "command center", "proof trail" or similar filler.
- Action labels read like sentences: "Read the documentation", "Inspect the source", "Open the live demo".

## Review checklist

- `bun run build` in `docs/` and `bun run test` in `src/WebClient/` pass; `python3 scripts/check-docs-drift.py --built-site` passes.
- Look at both themes on home, docs index, one article, sign-in, the workspace home, one CRUD list and one dialog, at desktop and phone widths. jsdom output is not a browser check.
- Stop temporary preview servers afterwards.
