# Qwik WebClient Rebuild Implementation Plan

> **For Hermes:** Use subagent-driven-development skill to implement this plan task-by-task.

**Goal:** Replace the existing Blazor/MudBlazor `src/WebClient` with a Qwik + Qwik City + Tailwind CSS client that implements the WebApi CRUD surface.

**Architecture:** The new WebClient is a Node-built Qwik application served from the existing `webclient-cpnucleo` container slot on port 5030. It uses Qwik City for routing, a small typed REST client for `WebApi` and `IdentityApi`, shared entity metadata to generate consistent CRUD pages, and Catalyst-inspired Tailwind components reimplemented in Qwik. The .NET `WebApi`, `IdentityApi`, `GrpcServer`, `Application`, `Domain`, and `Infrastructure` projects remain unchanged unless a CI or routing integration requires a narrow update.

**Tech Stack:** Qwik, Qwik City, TypeScript, Vite, Tailwind CSS, Docker multi-stage Node build, Vitest, Playwright, Testing Library, generated or hand-authored WebApi types, Catalyst-inspired component vocabulary.

---

## Planning progress snapshot

| Field | Status |
|---|---|
| Repo/path inspected | `/opt/data/github/jonathanperis/cpnucleo` |
| Branch/ref inspected | Created `plan/qwik-webclient-rebuild` from `origin/main` |
| Working tree before writing plan | Only existing untracked `.hermes/` content |
| Context loaded | `PRODUCT.md` and `DESIGN.md` loaded with Impeccable context loader |
| Impeccable register used | Product register, because this work is an authenticated task UI, not the docs brand site |
| Prior plans consulted | `.hermes/plans/fastendpoints-audit-plan-report.md` exists but is unrelated to this rebuild |
| Code changed in this planning pass | No product code deleted or modified yet. This plan file is the only intended new artifact |

## Evidence gathered

- Existing `src/WebClient` is a .NET 10 Blazor/MudBlazor web app with `WebClient.csproj`, Razor components, Kiota references, OpenTelemetry setup, and a .NET runtime Dockerfile.
- `cpnucleo.slnx` currently includes `src\WebClient\WebClient.csproj`. The rebuild must remove or replace that solution reference because a Qwik app has no `.csproj`.
- CI currently treats `WebClient` as a .NET matrix service in `.github/workflows/build-check.yml` and `.github/workflows/main-release.yml`. These workflows must get a WebClient-specific Node path.
- Docker Compose expects `webclient-cpnucleo` on internal port `5030` and healthchecks `/healthz`, published as host port `5400`.
- `WebApi` uses global route prefix `api`; REST routes are `/api/<resource>` and `/api/<resources>`.
- WebApi exposes 11 resources, each with 5 CRUD endpoints: `Organization`, `Project`, `Assignment`, `AssignmentType`, `Impediment`, `AssignmentImpediment`, `Appointment`, `Workflow`, `User`, `UserAssignment`, and `UserProject`.
- `IdentityApi` exposes `/api/login` on port 5010, with request `{ login, password }` and response `{ token }`.
- User explicitly requested Qwik docs, Tailwind, Catalyst, and Tailwind Plus UI Blocks as design references.

## External framework notes

- Qwik should be initialized as the application framework with Qwik City routing and Vite build output.
- Tailwind should be installed as the CSS system for Qwik and wired into the global stylesheet.
- Catalyst is a React/Tailwind UI kit. Do not import Catalyst React components into Qwik. Reimplement the component patterns in Qwik: app shell, sidebar, dropdowns, dialogs, fieldsets, buttons, tables, badges, pagination, and form controls.
- Tailwind Plus and Catalyst examples may be license-gated. Use them as visual and interaction references only unless Jonathan provides licensed source in the repository.

## Product design direction, Impeccable

**Physical scene:** A project lead uses CPnucleo on a laptop during planning and review sessions, switching between projects, assignments, users, appointments, and workflow states while keeping enough density to operate quickly.

**Theme decision:** Use a light-first product interface with restrained neutrals, not the dark docs workbench theme. The app is daytime operational software with form-heavy workflows. Dark mode can be added via tokens later, but phase 1 should ship one polished light theme.

**Color strategy:** Restrained. Use tinted neutral surfaces plus one cyan/blue accent for primary actions, current navigation, focus rings, and selected rows. Use semantic colors only for status, destructive actions, validation, and empty/error states.

**Visual language:** Catalyst-inspired, not cloned. Use precise borders, quiet shadow, excellent focus states, dense tables, command-style page headers, practical empty states, and responsive side navigation. Avoid gradient text, side-stripe callouts, glassmorphism, over-decorated cards, and identical icon-card grids.

**Information architecture:**

- `/` dashboard: operational overview, recent projects, workload preview, quick create buttons, API status.
- `/login`: IdentityApi login form with validation and inline error state.
- `/organizations`: organization CRUD.
- `/projects`: project CRUD with organization relation picker.
- `/assignments`: assignment CRUD with project, workflow, user, and assignment type relation pickers.
- `/workflows`: workflow CRUD and ordering affordance.
- `/users`: user CRUD, password fields hidden from table views.
- `/appointments`: appointment CRUD with assignment and user relation pickers.
- `/settings/types`: assignment type and impediment CRUD.
- `/settings/relations`: user-project, user-assignment, and assignment-impediment join CRUD.
- `/api-health`: health and environment diagnostics for WebApi and IdentityApi.

## Non-goals for phase 1

- Do not redesign WebApi, IdentityApi, GrpcServer, database schema, or backend endpoints.
- Do not add new backend endpoints to make the UI easier unless the implementation discovers a hard blocker and Jonathan approves it.
- Do not import React Catalyst code directly.
- Do not preserve any Blazor, Razor, MudBlazor, Kiota C# client, or .NET WebClient runtime files.
- Do not merge the implementation PR without explicit approval from Jonathan in the current conversation.

---

## Task 1: Create the implementation branch and demolition commit

**Objective:** Start from current `main`, remove the Blazor WebClient cleanly, and keep a reversible commit boundary.

**Files:**
- Delete contents of `src/WebClient/` except keep the directory itself.
- Modify: `cpnucleo.slnx`

**Steps:**

1. Ensure `main` is current:

```bash
git fetch origin main
git switch main
git pull origin main
git switch -c feat/qwik-webclient-rebuild
```

2. Delete existing WebClient implementation:

```bash
git rm -r src/WebClient/*
```

3. Remove the solution project reference:

```xml
<Project Path="src\WebClient\WebClient.csproj" />
```

from `cpnucleo.slnx`.

4. Verify:

```bash
git status --short
git diff --stat
```

Expected: only `src/WebClient/**` deletions and `cpnucleo.slnx` edit.

5. Commit:

```bash
git add cpnucleo.slnx src/WebClient
git commit -m "chore: remove Blazor WebClient shell"
```

---

## Task 2: Scaffold Qwik City in `src/WebClient`

**Objective:** Create a minimal Qwik app in the existing service location.

**Files:**
- Create: `src/WebClient/package.json`
- Create: `src/WebClient/tsconfig.json`
- Create: `src/WebClient/vite.config.ts`
- Create: `src/WebClient/src/root.tsx`
- Create: `src/WebClient/src/entry.dev.tsx`
- Create: `src/WebClient/src/entry.ssr.tsx`
- Create: `src/WebClient/src/routes/layout.tsx`
- Create: `src/WebClient/src/routes/index.tsx`
- Create: `src/WebClient/src/global.css`

**Steps:**

1. Use Qwik City as the baseline, but keep paths under `src/WebClient`.
2. Package scripts must include:

```json
{
  "scripts": {
    "dev": "vite --host 0.0.0.0 --port 5030",
    "build": "qwik build",
    "preview": "vite preview --host 0.0.0.0 --port 5030",
    "typecheck": "tsc --noEmit",
    "test": "vitest run",
    "test:e2e": "playwright test"
  }
}
```

3. Add dependencies for Qwik, Qwik City, Vite, TypeScript, Tailwind, PostCSS, Autoprefixer, Testing Library, Vitest, Playwright, and a small icon set such as `lucide-qwik` if available. If `lucide-qwik` is not maintained, use inline SVG icon components.
4. Verify:

```bash
cd src/WebClient
npm install
npm run typecheck
npm run build
```

5. Commit:

```bash
git add src/WebClient package-lock.json
git commit -m "feat: scaffold Qwik WebClient"
```

---

## Task 3: Wire Tailwind design tokens

**Objective:** Establish a Catalyst-inspired Tailwind theme and base component vocabulary.

**Files:**
- Create: `src/WebClient/tailwind.config.ts`
- Create: `src/WebClient/postcss.config.cjs`
- Modify: `src/WebClient/src/global.css`

**Token direction:**

```ts
colors: {
  canvas: 'oklch(var(--canvas) / <alpha-value>)',
  surface: 'oklch(var(--surface) / <alpha-value>)',
  raised: 'oklch(var(--raised) / <alpha-value>)',
  ink: 'oklch(var(--ink) / <alpha-value>)',
  muted: 'oklch(var(--muted) / <alpha-value>)',
  line: 'oklch(var(--line) / <alpha-value>)',
  accent: 'oklch(var(--accent) / <alpha-value>)',
  danger: 'oklch(var(--danger) / <alpha-value>)',
  success: 'oklch(var(--success) / <alpha-value>)'
}
```

**CSS variables:**

```css
:root {
  --canvas: 97% 0.006 250;
  --surface: 99% 0.004 250;
  --raised: 100% 0.005 250;
  --ink: 18% 0.026 255;
  --muted: 48% 0.032 255;
  --line: 86% 0.018 250;
  --accent: 58% 0.16 225;
  --danger: 56% 0.18 25;
  --success: 55% 0.14 155;
}
```

**Verification:**

```bash
cd src/WebClient
npm run build
```

Expected: Tailwind classes are compiled into the production bundle.

---

## Task 4: Build the app shell

**Objective:** Implement the persistent product navigation shell.

**Files:**
- Create: `src/WebClient/src/components/app-shell/app-shell.tsx`
- Create: `src/WebClient/src/components/app-shell/sidebar.tsx`
- Create: `src/WebClient/src/components/app-shell/topbar.tsx`
- Create: `src/WebClient/src/components/app-shell/mobile-navigation.tsx`
- Modify: `src/WebClient/src/routes/layout.tsx`

**Design requirements:**

- Sidebar groups: Work, People, Configuration, System.
- Topbar: current page title, environment chip, API status chip, profile/login affordance.
- Mobile: off-canvas navigation with `aria-expanded`, escape-close, and focus return.
- Active route state must be visible without using colored side stripes.

**Verification:**

```bash
cd src/WebClient
npm run typecheck
npm run build
```

Manual browser check: desktop sidebar, mobile nav, keyboard tab order.

---

## Task 5: Build the reusable component system

**Objective:** Reimplement Catalyst-like primitives in Qwik.

**Files:**
- Create under `src/WebClient/src/components/ui/`:
  - `button.tsx`
  - `input.tsx`
  - `textarea.tsx`
  - `select.tsx`
  - `field.tsx`
  - `badge.tsx`
  - `table.tsx`
  - `pagination.tsx`
  - `empty-state.tsx`
  - `alert.tsx`
  - `drawer.tsx`
  - `confirm-action.tsx`
  - `skeleton.tsx`

**Component standards:**

- Every interactive component has default, hover, focus-visible, disabled, loading, and error states where relevant.
- Dialog-like behavior should be used only for destructive confirmations and small create/edit flows. Prefer route-level forms for complex entities.
- Components must be accessible by keyboard and screen reader labels.

**Testing:**

Add Vitest tests for button variants, form field error rendering, table empty state, and confirmation action callbacks.

---

## Task 6: Create typed WebApi metadata

**Objective:** Capture the 11 WebApi resources and fields in one source of truth.

**Files:**
- Create: `src/WebClient/src/lib/api/resource-metadata.ts`
- Create: `src/WebClient/src/lib/api/types.ts`

**Resources:**

```ts
export const resources = [
  'organizations',
  'projects',
  'assignments',
  'assignmentTypes',
  'impediments',
  'assignmentImpediments',
  'appointments',
  'workflows',
  'users',
  'userAssignments',
  'userProjects'
] as const;
```

**Field requirements:**

- Include ID fields as `guid` inputs with relation select support.
- Hide passwords from table cells.
- Treat soft-delete behavior as backend-owned. Use delete endpoints, do not hard-delete locally.
- Pagination shape must handle `PaginatedResult<T>` from the API.

**Verification:**

```bash
cd src/WebClient
npm run typecheck
npm test
```

---

## Task 7: Implement REST client and environment config

**Objective:** Centralize HTTP behavior for WebApi and IdentityApi.

**Files:**
- Create: `src/WebClient/src/lib/config.ts`
- Create: `src/WebClient/src/lib/api/http-client.ts`
- Create: `src/WebClient/src/lib/api/webapi-client.ts`
- Create: `src/WebClient/src/lib/api/identity-client.ts`

**Config:**

Use public runtime variables that can be injected by Docker entrypoint or static config:

- `PUBLIC_WEBAPI_BASE_URL`, default `http://localhost:9999/api` for local browser use.
- `PUBLIC_IDENTITY_API_BASE_URL`, default `http://localhost:5200/api`.

**Client behavior:**

- Attach JWT from storage when present.
- Normalize 400, 404, 409, 429, and 500 responses to a typed `ApiError`.
- Support list pagination params: page number and page size.
- Include request abort support for route loaders.

**Testing:**

Use Vitest fetch mocks for success, validation error, not found, unauthorized, and rate limited responses.

---

## Task 8: Implement authentication flow

**Objective:** Add login, logout, token persistence, and protected app affordances.

**Files:**
- Create: `src/WebClient/src/routes/login/index.tsx`
- Create: `src/WebClient/src/lib/auth/auth-store.ts`
- Create: `src/WebClient/src/lib/auth/auth-guard.ts`
- Modify: `src/WebClient/src/components/app-shell/topbar.tsx`

**Behavior:**

- Form fields: login, password.
- Submit to `POST /api/login` on IdentityApi.
- Store token in `sessionStorage` first. Avoid `localStorage` until persistence is explicitly requested.
- Show inline validation errors, invalid credentials, and rate limit state.
- Because current WebApi endpoints are `AllowAnonymous`, auth gates are UI affordances in phase 1, not backend authorization.

**Verification:**

```bash
cd src/WebClient
npm test
npm run build
```

Manual: login success with seeded user `test-user` and `not-too-strong-password` when local services are available.

---

## Task 9: Build generic CRUD route factory

**Objective:** Avoid duplicating 11 nearly identical CRUD screens.

**Files:**
- Create: `src/WebClient/src/features/crud/crud-page.tsx`
- Create: `src/WebClient/src/features/crud/crud-table.tsx`
- Create: `src/WebClient/src/features/crud/crud-form.tsx`
- Create: `src/WebClient/src/features/crud/use-resource-data.ts`

**Behavior:**

- List page with search placeholder, pagination, table, skeleton state, empty state, and error recovery.
- Create/edit form generated from metadata.
- Details drawer for quick inspect.
- Delete confirmation with resource name and ID.
- Relation fields load options from their referenced resources.

**Verification:**

Vitest coverage for field rendering, relation option loading, delete confirmation, and API error rendering.

---

## Task 10: Implement primary work routes

**Objective:** Ship high-value entities first.

**Files:**
- Create route directories:
  - `src/WebClient/src/routes/organizations/index.tsx`
  - `src/WebClient/src/routes/projects/index.tsx`
  - `src/WebClient/src/routes/assignments/index.tsx`
  - `src/WebClient/src/routes/workflows/index.tsx`

**Acceptance:**

Each route uses the generic CRUD page but supplies page-specific copy, table columns, default sort, and relation fields.

**Verification:**

```bash
cd src/WebClient
npm run typecheck
npm test
npm run build
```

---

## Task 11: Implement people and scheduling routes

**Objective:** Add users, appointments, and user assignment/project links.

**Files:**
- Create route directories:
  - `src/WebClient/src/routes/users/index.tsx`
  - `src/WebClient/src/routes/appointments/index.tsx`
  - `src/WebClient/src/routes/settings/relations/index.tsx`

**Acceptance:**

- User table never displays password values.
- Appointment form includes date/time and duration inputs.
- Join resources communicate what each side of the relationship means.

---

## Task 12: Implement configuration routes

**Objective:** Add assignment types, impediments, and assignment impediments.

**Files:**
- Create route directories:
  - `src/WebClient/src/routes/settings/types/index.tsx`
  - `src/WebClient/src/routes/settings/impediments/index.tsx`

**Acceptance:**

- Assignment type and impediment screens are compact configuration tables.
- Assignment impediments support description plus assignment and impediment relation selectors.

---

## Task 13: Implement dashboard and API health route

**Objective:** Give the app a useful landing surface and operational diagnostics.

**Files:**
- Modify: `src/WebClient/src/routes/index.tsx`
- Create: `src/WebClient/src/routes/api-health/index.tsx`

**Dashboard modules:**

- Recent projects.
- Assignments due soon.
- Workflow distribution.
- Quick create actions.
- API health summary.

**Health checks:**

- `GET <WebApi base without /api>/healthz`
- `GET <IdentityApi base without /api>/healthz`

---

## Task 14: Add Docker and health endpoint support

**Objective:** Preserve Compose and CI expectations for `webclient-cpnucleo`.

**Files:**
- Create: `src/WebClient/Dockerfile`
- Create: `src/WebClient/nginx.conf` or a small Node server entry, depending on chosen runtime
- Create: `src/WebClient/public/healthz` if static NGINX serving is chosen

**Preferred container:**

Use a multi-stage Dockerfile:

1. `node:22-alpine` builder.
2. `npm ci`.
3. `npm run build`.
4. `nginx:1.27-alpine` final image serving static output on port 5030.
5. Serve `/healthz` as a 200 response.

**Verification:**

```bash
docker build -f src/WebClient/Dockerfile -t cpnucleo-webclient-qwik ./src/WebClient
docker run --rm -p 5400:5030 cpnucleo-webclient-qwik
curl -f http://localhost:5400/healthz
curl -f http://localhost:5400/
```

---

## Task 15: Update CI and release workflows

**Objective:** Replace .NET WebClient build logic with Node WebClient build logic while keeping other services unchanged.

**Files:**
- Modify: `.github/workflows/build-check.yml`
- Modify: `.github/workflows/main-release.yml`

**Plan:**

- Split WebClient out of the .NET matrix or add conditional steps.
- For WebClient setup-build-test, use Node 22, `npm ci`, `npm run typecheck`, `npm test`, and `npm run build` from `src/WebClient`.
- Keep container healthcheck matrix entry for `webclient-cpnucleo` on port 5400.
- Keep GHCR image name `ghcr.io/jonathanperis/cpnucleo-web-client`.

**Verification:**

```bash
git diff --check
npm --prefix src/WebClient ci
npm --prefix src/WebClient run typecheck
npm --prefix src/WebClient test
npm --prefix src/WebClient run build
dotnet test tests/Architecture.Tests/Architecture.Tests.csproj --no-restore
docker compose config --quiet
```

---

## Task 16: Update repository docs and context

**Objective:** Stop docs from claiming WebClient is Blazor/MudBlazor after the rebuild.

**Files to inspect and update:**
- `README.md`
- `AGENTS.md`
- `PRODUCT.md`
- `DESIGN.md`
- `docs/**`
- Any architecture diagrams or tables mentioning Blazor, MudBlazor, Kiota, or `.NET WebClient`

**Required wording:**

- Runtime: Qwik + Qwik City + Tailwind CSS.
- UI inspiration: Catalyst-inspired Tailwind product UI, reimplemented in Qwik.
- Container: static build served on port 5030 behind existing Compose/GHCR service name.

**Verification:**

```bash
git grep -n "Blazor\|MudBlazor\|Kiota\|WebAssembly" README.md AGENTS.md PRODUCT.md DESIGN.md docs src/WebClient
```

Expected: references are either removed or clearly historical where appropriate.

---

## Task 17: Add browser and accessibility smoke tests

**Objective:** Verify that the rebuilt client behaves like a product UI, not only a compiled bundle.

**Files:**
- Create: `src/WebClient/playwright.config.ts`
- Create: `src/WebClient/tests/e2e/navigation.spec.ts`
- Create: `src/WebClient/tests/e2e/crud-empty-states.spec.ts`
- Create: `src/WebClient/tests/e2e/login.spec.ts`

**Coverage:**

- Home loads.
- Sidebar navigation works on desktop.
- Mobile navigation opens and closes with keyboard support.
- Login renders validation and failed-auth state.
- One CRUD route renders loading, empty, and API-error state.
- No console errors on initial load.

**Verification:**

```bash
cd src/WebClient
npm run test:e2e
```

---

## Task 18: Full local validation and PR

**Objective:** Prove the rebuild is coherent before opening the PR.

**Commands:**

```bash
git diff --check
npm --prefix src/WebClient run typecheck
npm --prefix src/WebClient test
npm --prefix src/WebClient run build
dotnet test tests/Architecture.Tests/Architecture.Tests.csproj --no-restore
docker compose config --quiet
docker compose up webclient-cpnucleo -d --build --force-recreate --no-deps
curl -f http://localhost:5400/healthz
curl -f http://localhost:5400/
docker compose down
```

If a local Docker daemon is unavailable in the hosted container, run the non-Docker gates locally and rely on GitHub Actions for container healthcheck.

**PR checklist:**

- Summary mentions complete WebClient stack replacement.
- Call out deleted Blazor/MudBlazor/Kiota WebClient code.
- Include Qwik, Tailwind, Docker, CI, route coverage, and verification commands.
- Mark Tailwind Catalyst as design inspiration, not imported React code.
- Do not merge without explicit current-turn approval from Jonathan.

---

## Open questions to resolve during implementation

1. Should WebApi browser access go through NGINX at `http://localhost:9999/api` locally, or should Qwik call `webapi1-cpnucleo:5000/api` only server-side? Default plan: browser calls NGINX locally and production URL via `PUBLIC_WEBAPI_BASE_URL`.
2. Should we keep generated TypeScript API clients from FastEndpoints, or hand-author a small typed client? Default plan: hand-author metadata and client first, then consider generation if the generated output is stable for Qwik.
3. Should phase 1 include dark mode? Default plan: no, because a single polished light product theme is less risky.
4. Should CRUD forms be route-level pages or drawers? Default plan: drawers for simple config resources, route-level panels for assignments and appointments.

## Final acceptance criteria

- `src/WebClient` contains no Blazor, Razor, MudBlazor, C#, `.csproj`, or .NET runtime artifacts.
- Qwik app builds with TypeScript and Tailwind.
- All 11 WebApi resources have usable list, create, edit, details, and delete flows.
- Login flow calls IdentityApi and stores JWT for future authenticated requests.
- Docker image serves the app on port 5030 and `/healthz` returns 200.
- CI builds WebClient with Node instead of .NET and preserves container healthcheck.
- Docs and repo metadata no longer describe WebClient as Blazor/MudBlazor.
- Product UI passes Impeccable guardrails: no gradient text, no side-stripe callouts, no glassmorphism default, no identical card grids, no inconsistent component states.
