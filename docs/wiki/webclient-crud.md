# WebClient CRUD

The WebClient under `src/WebClient` is an Astro + Qwik frontend that exposes authenticated, Catalyst-inspired CRUD screens for the WebApi resources listed in `src/WebClient/src/lib/api/resource-metadata.ts`.

---

## Resource metadata

`resource-metadata.ts` is the source of truth for each screen:

- route path, singular label, plural label, and description
- item and list API paths (`/api/{entity}` and `/api/{entities}` through the configured WebApi base URL)
- table fields, form fields, required fields, and read-only fields
- relation fields such as `projectId`, `workflowId`, `userId`, and `organizationId`

The active resources are organizations, projects, assignments, assignment types, impediments, assignment impediments, appointments, workflows, users, user assignments, and user projects.

---

## List pages and relation labels

`CrudPage` renders each list with pagination, details, edit, and delete actions. It also owns prefilled edit forms and relation fields are displayed as readable relation labels instead of raw GUIDs when the related record can be resolved.

Relation resolution works in two passes:

1. Prefetch the first 100 records for each relation used by the current resource.
2. Scan the visible rows for missing related IDs and fetch those records on demand.

Fetched relation records are merged, not replaced, so an on-demand lookup for an off-page relation is not lost when the first-page prefetch finishes later.

Readable labels are produced by `displayEntityLabel`:

| Related record shape | Display label |
|----------------------|---------------|
| `name` + different `description` | `Name — Description` |
| `name` + different `login` | `Name (login)` |
| only `name`, `description`, or `login` | that value |
| unresolved relation | raw ID fallback |

---

## Create and edit forms

Forms are generated from `formFields(resource)` and reuse the same metadata as list pages.

Edit mode pre-fills controls from the selected row:

- text, textarea, GUID, and number fields are converted to strings
- `date` fields are normalized to `YYYY-MM-DD`
- `datetime-local` fields are normalized to `YYYY-MM-DDTHH:mm`
- relation selects are controlled by the selected row's relation ID

A relation select is disabled only while its option collection is still loading. If the selected relation ID is outside the prefetched option page, the form keeps a fallback option for that ID so saving the edit does not accidentally drop the existing relationship.

Empty optional GUID, number, date, and datetime fields are omitted from save payloads. Number fields are converted back to numbers before sending to the WebApi.

---

## WebApi response normalization

The WebClient accepts the response shapes currently used by the WebApi:

- list arrays
- paginated list objects
- `{ result: ... }` envelopes
- singular item objects with `id`
- singular `{ result: item }` envelopes
- singular resource-key envelopes such as `{ organization: { ... } }`

`normalizeItem` unwraps singular item envelopes before relation-label lookup code receives the record. This keeps relation labels readable even when an on-demand lookup returns an entity-specific envelope instead of the entity at the top level.

---

## Regression coverage

The WebClient Vitest suite covers the CRUD behaviors that are easy to regress:

- edit field value formatting for date and datetime controls
- controlled relation select binding in edit forms
- fallback selected relation options when the related record is not in the prefetched page
- readable relation-label formatting and merged relation record caches
- singular item response normalization across raw, `result`, and resource-key envelopes

Run the WebClient gate from `src/WebClient`:

```bash
bun test
bun run typecheck
bun run build
```
