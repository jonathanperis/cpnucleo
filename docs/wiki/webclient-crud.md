# WebClient CRUD

The WebClient uses Astro static routes, native TypeScript and Tailwind CSS. There is no client framework runtime. Resource metadata generates the eleven CRUD routes and their forms; `[resource].astro` preserves the existing public paths.

## Implementation

- `features/crud/CrudPage.astro`: semantic HTML, labels, forms, table, pagination landmark and native details dialog.
- `features/crud/crud-controller.ts`: form events, loading, safe DOM rendering, pagination, cancellation and relation selection.
- `lib/api/webapi-client.ts`: HTTP/SSE contracts and singular item response normalization.
- `lib/api/http-client.ts`: bearer handling, errors, inactivity expiry and refresh.
- `components/AuthGuard.astro`: authenticated workspace visibility and session lifecycle.
- `components/LoginForm.astro`: native login form, with empty credentials.

## Editing and relationships

Screens provide prefilled edit forms and readable relation labels. Relation searches are server-side and paginated in batches of 100. Missing labels are fetched as batched ID lookups, not one network request for every cell. A selected relation remains available when it falls outside the first page.

Password inputs are blank and never displayed in tables/details. They are required when creating a user and optional when editing one. User administration requires an administrator; an unavailable count or relation is shown as unavailable rather than fabricated as zero.

Hours and order controls use whole numbers. Calendar datetime controls explicitly display UTC; outgoing date/timestamp values include a UTC offset. Version-aware project edits send the last observed timestamp and display a conflict when another writer has changed the project.

## Lists and real-time behavior

JSON loads the initial page; SSE supplies subsequent snapshots. The server refreshes at least every 15 seconds, including writes received by another instance or gRPC. Connections retry with bounded backoff. Navigation/disposal aborts the active request, and old page responses cannot replace a newer page.

The details view uses native `<dialog>` behavior for focus, Escape and modality. API-controlled strings are assigned with `textContent` rather than interpolated into HTML. Errors use alert semantics; counts and page status use live regions.

## Environment and verification

Default API addresses are localhost. Production Docker builds receive explicit public `PUBLIC_*` arguments. Changing container environment variables after a static build does not rewrite its JavaScript.

`bun run test` builds all routes and then runs native DOM interaction tests against generated pages. Browser rendering, screen-reader behavior and visual contrast still require a real-browser/manual accessibility review.
