# WebClient CRUD

The WebClient uses Astro static routes, native TypeScript and Tailwind CSS. There is no client framework runtime. Resource metadata generates the eleven CRUD routes and their forms; `[resource].astro` preserves the existing public paths. Every page is an `.astro` file; see [Astro-only pages](#astro-only-pages).

## Implementation

- `features/crud/CrudPage.astro`: semantic HTML, labels, per-field error slots, forms, table, pagination landmark and native details dialog.
- `features/crud/crud-controller.ts`: form events, loading, safe DOM rendering, pagination, cancellation, relation selection, authorization gating and focus management.
- `lib/api/webapi-client.ts`: HTTP/SSE contracts, flat list parameters and singular item response normalization.
- `lib/api/http-client.ts`: bearer handling, the error envelope, inactivity expiry, refresh, session claims and cross-tab logout.
- `components/AuthGuard.astro`: authenticated workspace visibility and session lifecycle.
- `components/LoginForm.astro`: native login form, with empty credentials.
- `scripts/csp.mjs`, `scripts/static-server.mjs`, `scripts/preview.mjs`: build-time CSP manifest and the Node static server.

The former `/settings/types/` and `/settings/relations/` pages were removed. They stacked several CRUD screens (and several live streams) on one unlinked page, duplicating the routes in the Setup and People navigation groups, and were absent from the login return-route list.

## Editing and relationships

Screens provide prefilled edit forms and readable relation labels. Relation searches are server-side and paginated in batches of 100. Missing labels are fetched as batched ID lookups, not one network request for every cell.

A selected relation keeps its human label when it falls outside the loaded search page: the picker looks it up in the records already cached from table lookups and earlier searches before falling back to the raw ID. The control's current value is authoritative, so choosing "Select …" stays cleared after later searches. Each relation has one in-flight search: a new query aborts the previous request, late responses are ignored, and "More" always continues the query its page counter belongs to, even if the search box was edited since.

Password inputs are blank and never displayed in tables/details. They are required when creating a user and optional when editing one.

Hours and order controls use whole numbers of at least 1; workflow order is required. Task end dates must be on or after the start date; the form checks this before calling the API (the domain enforces it again). Calendar datetime controls explicitly display UTC; outgoing date/timestamp values are ISO UTC strings ending in `Z`. Version-aware project edits send the last observed timestamp.

## Authorization in the UI

The UI reads the JWT claims after sign-in: `sub` (user id), `cpnucleo:login` and `cpnucleo:admin`. This only shapes the interface; WebApi and IdentityApi enforce authorization.

- Organizations, progress steps, task types, blockers and team members are administrator-managed. Non-admin sessions see an explanatory note instead of create/edit/delete controls and can still browse the records.
- The team member list is admin-only. Non-admin sessions do not request `/api/users`: the Team members page explains that the list requires administrator access, the home counter shows "Admin only", and person pickers (task owner, calendar person, people on tasks/projects) offer only the signed-in user, labelled with the login claim.
- New calendar items default to the signed-in user.
- Project-scoped records (projects, tasks, calendar items and their links) are filtered by the API to projects the user belongs to; hidden records behave like missing ones.

## Errors

Non-2xx responses use `{ "statusCode", "message", "errors"? }`. Field messages under `errors` (camelCase property names) and `errors.generalErrors` are shown in preference to the generic `message`. Matching form controls get `aria-invalid="true"` and an `aria-describedby` link to their message; focus moves to the first invalid control, and the markers clear on the next submit.

- **401** ends the session (cleared storage, redirect to login with a return URL) for JSON requests, live streams and refreshes alike. A refresh rejected after a password or login change is handled the same way.
- **403** shows the permission message and keeps the session.
- **404 / 409** show the server message, for example an optimistic-concurrency conflict on a project or a removal blocked by active related records. Conflicts are detected by status, not by a `success` flag. After a project conflict the form keeps your edits and loads the current version, so the next save is a deliberate overwrite instead of failing until the dialog is reopened.
- **429** shows the message plus the `Retry-After` delay (for example a sign-in lockout). Browsers can only read that header cross-origin when the API lists it in `Access-Control-Expose-Headers`.

Removal sends `{ "ids": [...] }` with 1–100 distinct IDs; the server applies it atomically.

## Lists and real-time behavior

List requests send each parameter once with flat keys (`pageNumber`, `pageSize`, `search`, `ids`) and an explicit stable order (`sortColumn=CreatedAt&sortOrder=ASC`).

Each listing opens one SSE request. The server's first event is the initial snapshot, so no separate JSON request is made on load or on reconnect; later events arrive on changes or at least every 15 seconds, including writes received by another instance or gRPC. A server that answers without SSE is treated as one non-live snapshot. Backoff resets only after a live snapshot arrives; a stream that ends immediately, a non-SSE response or a transport error backs off exponentially (1 s doubling to a 15 s cap, with jitter, and never sooner than a `Retry-After`). 400/401/403/404 are not retried. Navigation/disposal aborts the active request, and old page responses cannot replace a newer page.

## Session

Only real input (pointer, keyboard, touch, scroll) counts as activity. API calls, stream reconnects and background token refreshes do not extend the 15-minute inactivity window, and the inactivity timer re-checks the last recorded user activity before signing out. Logging out in one tab signs out the other tabs of the origin through `BroadcastChannel`, with a `storage`-event fallback. Tokens live in `sessionStorage`, so each tab signs in separately.

## Accessibility

The details view uses native `<dialog>` behavior for focus, Escape and modality. API-controlled strings are assigned with `textContent` rather than interpolated into HTML. Per-row actions have distinct accessible names such as "Edit Atlas" (link records use their related labels). When the form closes after save or cancel, focus returns to the control that opened it; if live updates replace that row, focus follows to the equivalent button, or to the create button/heading when the row is gone.

Errors use `role="alert"`. Routine progress such as "Loading options…" uses `role="status"`. The page summary is a polite live region updated only when its text changes, and the eleven home counters are not live regions: one status message summarizes them once loaded.

## Security headers and CSP

`astro build` runs a small integration (`scripts/csp.mjs`) that writes `dist/csp-manifest.json`:

- `connectSrc`: the origins of the build-time `PUBLIC_WEBAPI_BASE_URL` and `PUBLIC_IDENTITY_API_BASE_URL` (defaults `http://localhost:5100` and `http://localhost:5200`, the same defaults as `lib/config.ts`). The lab stack and production hosts therefore get the policy that matches their bundle.
- `scriptHashes`: SHA-256 hashes of every inline script in the generated HTML (the theme bootstrap and the small scripts Astro inlines).

`scripts/preview.mjs` refuses to start without the manifest, validates it and sends `script-src 'self' <hashes>` (no `'unsafe-inline'` for scripts), `connect-src 'self' <origins>`, `object-src 'none'`, `frame-ancestors 'none'` plus HSTS, `nosniff`, `X-Frame-Options` and `Referrer-Policy`. Styles still allow inline attributes. Because the manifest is generated into `dist/`, the existing image copies it with the build output. If a file read fails after the response headers were sent, the server destroys the response instead of writing headers twice.

## Astro-only pages

Every page in `src/WebClient/src/pages` and `docs/src/pages` is an `.astro` file, neither package depends on a client UI framework or framework integration, and neither `public/` directory contains HTML pages. Document layouts emit `<meta name="generator" content={Astro.generator}>`, and every built HTML file must carry that marker. The rule is enforced by `src/guarantees/astro-pages.test.ts` (sources and `dist/`), `scripts/check-docs-drift.py` (sources, plus `docs/out` with `--built-site`) and `tests/Architecture.Tests/AstroPagesTests.cs`.

## Environment and verification

Default API addresses are localhost. Production Docker builds receive explicit public `PUBLIC_*` arguments. Changing container environment variables after a static build rewrites neither its JavaScript nor its CSP manifest; rebuild instead.

`bun run test` builds all routes and then runs native DOM interaction tests against generated pages (see [Testing](../testing/)). jsdom is not a browser: rendering, screen-reader behavior and visual contrast still require a real-browser/manual accessibility review.
