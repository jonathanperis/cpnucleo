# WebClient CRUD

The WebClient uses Astro static routes, native TypeScript and Tailwind CSS. There is no client framework runtime. Resource metadata generates the eleven CRUD routes and their forms; `[resource].astro` preserves the existing public paths. Every page is an `.astro` file; see [Astro-only pages](#astro-only-pages).

Besides the CRUD routes there are work views built on the same list endpoints and filters: Home ("My work"), `/board/`, `/calendar/`, the project and task pages (`/projects/view/?id=…`, `/assignments/view/?id=…`) and `/account/`. See [Work views](#work-views).

## Implementation

- `features/crud/CrudPage.astro`: semantic HTML, labels, per-field error slots, forms, table, pagination landmark and native details and delete-confirmation dialogs.
- `features/crud/crud-controller.ts`: form events, loading, safe DOM rendering, pagination, search, sorting, filters, selection and bulk removal with undo, cancellation, relation selection, authorization gating and focus management.
- `features/crud/list-state.ts`, `features/crud/columns.ts`: the list state kept in the URL and the per-browser column choice.
- `features/board/`, `features/calendar/`, `features/records/`, `features/home/my-work.ts`, `features/account/`: the work views.
- `features/search/command-palette.ts`, `features/inspector/request-inspector.ts`, `lib/api/request-log.ts`: Ctrl/⌘+K search and the API request inspector.
- `lib/ui/toast.ts`, `lib/ui/menus.ts`, `lib/ui/theme.ts`, `components/SessionWarning.astro`: notifications, header menus, the theme choice and the inactivity warning.
- `lib/api/webapi-client.ts`: HTTP/SSE contracts, flat list parameters and singular item response normalization.
- `lib/api/http-client.ts`: bearer handling, the error envelope, inactivity expiry, refresh token rotation, sign-out (revocation and end session), session claims and cross-tab logout.
- `lib/api/oidc.ts`: the OpenID Connect client: authorize URL with PKCE (S256), state and nonce; code redemption and identity token checks on the callback page.
- `components/AuthGuard.astro`: authenticated workspace visibility and session lifecycle.
- `components/LoginForm.astro`, `features/auth/login-controller.ts`: the sign-in page. Opened by IdentityApi with a pending request, its native form posts the credentials to IdentityApi; opened directly, it starts a sign-in.
- `pages/signin-callback.astro`, `features/auth/signin-callback.ts`: the redirect URI. Removes the code from the address bar, redeems it and opens the page the sign-in started from.
- `lib/navigation.ts`, `components/SidebarNav.astro`: one navigation model for the sidebar, mobile menu, breadcrumb and home overview.
- `global.css`, `lib/icons.ts`, `components/Icon.astro`: design tokens, shared component classes and the stroke icon set used by templates and controllers.
- `scripts/csp.mjs`, `scripts/static-server.mjs`, `scripts/preview.mjs`: build-time CSP manifest and the Node static server.

## Visual design

The WebClient and the documentation site share one look, described in `DESIGN.md`: warm paper, near-black ink and a single vermilion accent, with serif titles. Paper is the default theme. The theme menu offers Paper, Night and System (follows the operating system, including changes while the page is open); the choice is stored per browser and applied by an inline script before first paint. Colors are OKLCH channel tokens in `global.css` (`--canvas`, `--surface`, `--raised`, `--ink`, `--muted`, `--subtle`, `--line`, `--accent`, `--action` and status colors), mapped into Tailwind so utilities accept alpha (`bg-accent/10`). Primary buttons are inked (`--action`); vermilion marks focus, the current place and the brand. Base element rules live in `@layer base` so Tailwind utilities always override them, and reusable pieces (`.btn`, `.field`, `.panel`, `.page-title`, `.stat-num`, `.data-table`, `.page-button`, `.modal`) live in `@layer components`.

Newsreader (titles, labels and figures), IBM Plex Sans (interface) and IBM Plex Mono (code and identifiers) are self-hosted from `@fontsource-variable/*` and `@fontsource/*` and bundled by Vite, so the CSP needs no external font origin. Lists lead with each record's own fields and show Created last; date-only fields render the UTC calendar day the form edits. Row actions are icon buttons with distinct accessible names and stay pinned to the right edge on wide tables. Below 640 px each row becomes a card of label/value pairs (the cells carry `data-label`), with the selection box, the record name and the actions on its first line. While a related name is loading the cell shows a placeholder, never the raw id; ids that stay unresolved (deleted, or not visible to the caller) read "Unavailable". Related projects and tasks are links to their pages. Motion is limited to short opacity/transform transitions and is disabled under `prefers-reduced-motion`.

The former `/settings/types/` and `/settings/relations/` pages were removed. They stacked several CRUD screens (and several live streams) on one unlinked page, duplicating the routes in the Setup and People navigation groups, and were absent from the login return-route list.

## Editing and relationships

Create and edit forms open in a modal sheet (full screen on small screens). Create forms also offer "Save and add another", which keeps the sheet open and prefilled from the list's filters. Closing a form with unsaved changes (Cancel, Escape, the backdrop) asks before discarding them, and leaving the page warns too. Saves and removals are confirmed with a notification; the saved row is briefly highlighted, and a new record is shown on page 1 because lists are newest first.

Relation pickers are native selects listed alphabetically. Their filter field and "More" appear only when the related list is longer than one page (100); filtering runs as the person types (300 ms pause) or on Enter, which never submits the form. Relation searches are server-side and paginated in batches of 100. Missing labels are fetched as batched ID lookups, not one network request for every cell.

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

Removal sends `{ "ids": [...] }` with 1–100 distinct IDs; the server applies it atomically. Rows can be selected (one page at a time) and removed together. Every removal notification offers Undo, which calls `POST /api/{entity}/restore` with the same ids.

## Lists and real-time behavior

List requests send each parameter once with flat keys (`pageNumber`, `pageSize`, `search`, `ids`, the relation filters such as `projectId`, and `dateFrom`/`dateTo`) and an explicit order. Listings default to newest first (`sortColumn=CreatedAt&sortOrder=DESC`); column headers sort by their persisted column (`aria-sort` reflects it); pickers and lookups use a stable ascending order.

The page, page size, search, sort and filters live in the address bar (`/assignments/?projectId=…&sort=endDate&order=asc&page=2`), so reloads, Back and shared links keep the view. Values the resource cannot use are dropped before any request. Active filters appear as removable chips, and a filtered list prefills new records with the same relation. Two one-time link parameters open forms: `?new=1` (create) and `?edit=<id>`. The search box filters after a short pause, and `/` focuses it. The Columns menu hides or shows columns per browser; tasks start without Description and Task type.

Each listing opens one SSE request. The server's first event is the initial snapshot, so no separate JSON request is made on load or on reconnect; later events arrive on changes or at least every 15 seconds, including writes received by another instance or gRPC. A server that answers without SSE is treated as one non-live snapshot. Backoff resets only after a live snapshot arrives; a stream that ends immediately, a non-SSE response or a transport error backs off exponentially (1 s doubling to a 15 s cap, with jitter, and never sooner than a `Retry-After`). 400/401/403/404 are not retried. Navigation/disposal aborts the active request, and old page responses cannot replace a newer page.

## Sign-in

The WebClient is the public OpenID Connect client `cpnucleo-webclient` of IdentityApi (see [API reference](../api-reference/)):

1. A protected page without a session goes to `/login/?returnUrl=…`, which starts the authorization code flow: it stores PKCE, state and nonce in this tab and opens IdentityApi's `/connect/authorize`.
2. Without an identity session, IdentityApi returns the browser to `/login/?authRequest=…` on the WebClient origin that started the request (when it is a configured origin). The native form posts to IdentityApi (CSP `form-action` allows only that origin), so the identity cookie is first-party there. Errors come back as `error=invalid` or `error=locked&retryAfter=N`.
3. IdentityApi redirects to `/signin-callback/` with a code. The page checks the state, redeems the code with the PKCE verifier, checks the identity token's issuer, audience and nonce, stores the tokens and opens the original page.

With an identity session (another tab already signed in), step 2 is skipped. Tokens never appear in URLs.

## Session

A minute before the inactivity sign-out, a dialog counts down and offers "Stay signed in"; any real input also dismisses it. Only real input (pointer, keyboard, touch, scroll) counts as activity. API calls, stream reconnects and background token refreshes do not extend the 15-minute inactivity window, and the inactivity timer re-checks the last recorded user activity before signing out. Refreshes use the one-time refresh token at the token endpoint; a rejected refresh (eight-hour limit, sign-out, credential change, replay) ends the session.

An expired session, an API 401 and an explicit sign-out all end the identity session through `/connect/endsession`, otherwise its cookie would sign the browser straight back in. Signing out also revokes the refresh token. Another tab that receives the sign-out shows `/login/?signedOut=1` and waits instead of signing in again automatically. Logging out in one tab signs out the other tabs of the origin through `BroadcastChannel`, with a `storage`-event fallback. Tokens live in `sessionStorage`, so each tab keeps its own; a new tab gets them through the existing identity session without asking for the password again.

## Accessibility

The details view and the delete confirmation use native `<dialog>` behavior for focus, Escape and modality; the confirmation replaces `window.confirm`, starts on Cancel and returns focus to the Delete button that opened it. API-controlled strings are assigned with `textContent` rather than interpolated into HTML. Per-row actions have distinct accessible names such as "Edit Atlas" (link records use their related labels). When the form closes after save or cancel, focus returns to the control that opened it; if live updates replace that row, focus follows to the equivalent button, or to the create button/heading when the row is gone.

Errors use `role="alert"`. Routine progress such as "Loading options…" uses `role="status"`; notifications use one polite region and pause their timer while hovered or focused. The search palette is a combobox with `aria-activedescendant`; arrow keys move through results, Enter opens one and Escape closes it. The page summary is a polite live region updated only when its text changes, and the eleven home counters are not live regions: one status message summarizes them once loaded.

## Work views

- **Home** shows the signed-in person's open tasks (overdue and due within seven days first), their hours this week and their projects, above the per-area counts.
- **Board** (`/board/?projectId=…`) has one column per progress step, ordered by `order`, and follows the same live listing stream as the Tasks list (up to 100 tasks, due first). Cards move by drag and drop or with each card's "Move to" select, the keyboard and screen reader path; a move is a regular task update and is undone with a notification if the API rejects it.
- **Calendar** (`/calendar/?week=YYYY-MM-DD`) shows calendar items Monday to Sunday with daily and weekly hour totals, through the `dateFrom`/`dateTo` filter. Days and times are UTC, like the forms. "Only my time" is on by default.
- **Project and task pages** summarize a record: a project's tasks by step, members and planned hours; a task's dates, hours logged against the estimate, blockers, people and time log. Their buttons open the filtered lists and forms (`/assignments/?projectId=…&new=1`).
- **Your account** (`/account/`) shows the profile and changes the display name (`PATCH /api/me`) or the password (`POST /api/me/password`). A password change ends every session of the account, so the page signs out and the sign-in page says why.
- **Search** (Ctrl/⌘+K, or the header button) lists matching pages and, after two characters, the first records of each readable area through the list endpoints' `search`.
- **API requests**: a header toggle opens a panel listing this tab's WebApi calls and listing streams (method, URL, status, time, snapshots). Headers and bodies are never recorded.

## Security headers and CSP

`astro build` runs a small integration (`scripts/csp.mjs`) that writes `dist/csp-manifest.json`:

- `formAction`: the IdentityApi origin, the only cross-origin form target.
- `connectSrc`: the origins of the build-time `PUBLIC_WEBAPI_BASE_URL` and `PUBLIC_IDENTITY_API_BASE_URL` (defaults `http://localhost:5100` and `http://localhost:5200`, the same defaults as `lib/config.ts`). The lab stack and production hosts therefore get the policy that matches their bundle.
- `scriptHashes`: SHA-256 hashes of every inline script in the generated HTML (the theme bootstrap and the small scripts Astro inlines).

`scripts/preview.mjs` refuses to start without the manifest, validates it and sends `script-src 'self' <hashes>` (no `'unsafe-inline'` for scripts), `connect-src 'self' <origins>`, `form-action 'self' <identity origin>` (the sign-in form posts to IdentityApi), `object-src 'none'`, `frame-ancestors 'none'` plus HSTS, `nosniff`, `X-Frame-Options` and `Referrer-Policy`. Styles still allow inline attributes. Because the manifest is generated into `dist/`, the existing image copies it with the build output. If a file read fails after the response headers were sent, the server destroys the response instead of writing headers twice.

## Astro-only pages

Every page in `src/WebClient/src/pages` and `docs/src/pages` is an `.astro` file, neither package depends on a client UI framework or framework integration, and neither `public/` directory contains HTML pages. Document layouts emit `<meta name="generator" content={Astro.generator}>`, and every built HTML file must carry that marker. The rule is enforced by `src/guarantees/astro-pages.test.ts` (sources and `dist/`), `scripts/check-docs-drift.py` (sources, plus `docs/out` with `--built-site`) and `tests/Architecture.Tests/AstroPagesTests.cs`.

## Environment and verification

Default API addresses are localhost. Production Docker builds receive explicit public `PUBLIC_*` arguments. Changing container environment variables after a static build rewrites neither its JavaScript nor its CSP manifest; rebuild instead.

`bun run test` builds all routes and then runs native DOM interaction tests against generated pages (see [Testing](../testing/)). jsdom is not a browser: rendering, screen-reader behavior and visual contrast still require a real-browser/manual accessibility review.
