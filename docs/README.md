# Documentation site

Astro static documentation and landing site, published at <https://jonathanperis.github.io/cpnucleo/>. The application UI is the separate `src/WebClient` project.

## Develop and verify

Use Node supported by the installed Astro version, Bun, and Python 3 for repository drift checks. Run from `docs/`:

```sh
bun install --frozen-lockfile
bun run dev
```

Development defaults to `http://localhost:4321/`. Production uses the `/cpnucleo` base:

```sh
bun run build
bun audit
python3 ../scripts/check-docs-drift.py --built-site
bun run preview
```

Open `http://localhost:4321/cpnucleo/` for production preview. `out/`, `.astro/` and `node_modules/` are generated and ignored. Run `python3 scripts/check-docs-drift.py` from the repository root for source-only checks.

## Content ownership

| Location | Purpose |
|---|---|
| `../README.md` | Introduction, quick start and verification entry points |
| `wiki/*.md` | Articles at `/docs/<filename>/`, including `home.md` as Project Overview |
| `src/pages/docs/[...slug].astro` | Documentation index, labels/summaries and rendering |
| `src/lib/sidebar.config.ts` | Navigation grouping and generated route order |
| `src/components/home/` | Landing copy, proof links and quick start |
| `src/layouts/BaseLayout.astro` | Metadata, canonical URLs and asset paths |
| `astro.config.mjs` | Static output, Pages base, sitemap and styling |
| `../scripts/generate_social_assets.py` | Source for social-preview/thumbnail PNGs and icon fallbacks |
| `adr/` | Repository-readable decisions; not published wiki routes |
| `IMPECCABLE_OVERHAUL_PLAN.md` | Historical proposal, not the current backlog |

To add an article, add its Markdown file, navigation ID, page label and summary. Link to siblings as `../getting-started/`; templates use `import.meta.env.BASE_URL`. Use absolute GitHub links for repository files that are not published assets. The drift checker validates navigation and, with `--built-site`, generated local links, fragments and assets.

Keep exact dependency versions in manifests/lockfiles. Link to source for operational claims and label experiments/history. The [September 2026 audit](audit-2026-09.md) records findings and follow-up work.

### Social images

When changing stack/hero copy, update the generator and regenerate the checked-in PNGs too. The generator requires Pillow from `scripts/requirements-social-assets.txt` and Linux Liberation/DejaVu fonts. A disposable generation environment, run from the repository root:

```sh
docker run --rm -v "$PWD:/work" -w /work python:3.14-slim sh -c 'pip install -r scripts/requirements-social-assets.txt && apt-get update -qq && apt-get install -y -qq fonts-liberation fonts-dejavu-core && python3 scripts/generate_social_assets.py'
```

It verifies dimensions/formats. Inspect the resulting image copy and layout before committing; a successful Astro build does not detect stale text embedded in PNGs.

## Publication and analytics

`.github/workflows/deploy.yml` calls a commit-pinned Pages workflow in `jonathanperis/.github`. It builds with Node and uploads `out/` on `main` pushes/manual dispatch. PR checks build/audit docs separately.

`PUBLIC_GA_ID` is an optional public GA4 measurement ID evaluated at build time. Copy `.env.example` to `.env` only when needed. The current Pages caller does not forward this optional secret, so CI publication does not enable analytics. Enabling it requires configuring and forwarding `PUBLIC_GA_ID` to the reusable workflow.

`?ab=proof`, `?ab=architecture` and `?ab=control` select visual previews. CTA events require configured analytics; the controls are not randomized experiments or evidence of a winning variant.
