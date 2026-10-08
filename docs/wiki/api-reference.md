# API Reference

Cpnucleo exposes three API services: the WebApi (REST), the IdentityApi (authentication), and the GrpcServer (gRPC command handling).

---

## WebApi -- REST Endpoints

The WebApi uses [FastEndpoints](https://fast-endpoints.com/) to define REST endpoints with Swagger/OpenAPI documentation. Each entity has 5 standard CRUD endpoints plus a REST-only restore endpoint; the signed-in user's own account has three self-service endpoints.

### Base URL

- Learning lab: `http://localhost:5100`
- Legacy load-balanced development stack: `http://localhost:9999`

### Swagger

Available at `/swagger`; `UseSwaggerGen()` is currently enabled in all environments. Request/response schemas are generated from the endpoint models.

### Endpoint Pattern

Every entity follows this consistent pattern:

| Method | Route | Description |
|--------|-------|-------------|
| `POST` | `/api/{entity}` | Create a new record from a JSON body |
| `GET` | `/api/{entity}?id={uuid}` | Get a single record by query-string ID |
| `GET` | `/api/{entities}` | List records (paginated; plural route such as `/api/projects`) |
| `PATCH` | `/api/{entity}` | Update from a JSON body containing `id` and the required resource fields |
| `DELETE` | `/api/{entity}` | Soft-delete IDs supplied as a JSON body: `{"ids":["uuid"]}` |
| `POST` | `/api/{entity}/restore` | Undo the soft delete of IDs supplied as a JSON body: `{"ids":["uuid"]}` (REST only) |

These routes do not contain an `/{id}` path segment. PATCH uses each resource's request DTO, not JSON Patch operations. Include `Authorization: Bearer <token>` on CRUD requests.

### Authorization

The same rules apply to REST and gRPC and to every persistence style:

| Resources | Read | Create, update, remove |
|-----------|------|------------------------|
| Organizations, Workflows, AssignmentTypes, Impediments (catalog) | Any authenticated user | Administrators |
| Users | Administrators | Administrators |
| Projects | Members of the project | Members; any authenticated user may create one and becomes a member |
| Assignments, UserProjects | Members of the row's project | Members of the project (both the old and the new project when moving a row) |
| Appointments, AssignmentImpediments, UserAssignments | Members of the assignment's project | Members; non-administrators may only record or change their own appointments |

Administrators (`cpnucleo:admin` claim backed by `CPNUCLEO_ADMIN_LOGINS`; when that list is empty, the seeded `demo@cpnucleo.local` account is the administrator) see and change everything. Rows the caller can't see behave like missing rows (404 on GET, excluded from lists); writes the caller isn't allowed to make return 403 (gRPC `PermissionDenied`).

### Removal

`DELETE` bodies contain 1–100 ids; duplicates are ignored. Every resource removes its batch atomically: if any id is missing or invisible, nothing changes and the response is 404. A record that still has active dependent data (an organization with projects, a project with assignments, an assignment with appointments, ...) returns 409 with the dependent kind in the message. Project and user memberships and user assignments are removed together with their project, assignment or user.

### Restore (undo a removal)

`POST /api/{entity}/restore` with `{"ids":["uuid"]}` undoes soft deletes for all eleven resources (for example `/api/organization/restore`, `/api/assignmentImpediment/restore`). It is REST-only; gRPC has no restore command. The body follows the removal rules: 1–100 distinct ids, applied atomically with the persistence style of the resource's removal. A restore sets `Active = true` and clears `DeletedAt`; nothing else changes, including `updatedAt`, so a project's `expectedVersion` stays valid.

| Status | When |
|--------|------|
| 200 `{"success":true}` | Every id was a removed row the caller may see and change |
| 400 | Invalid body (`errors.ids`), or a restored row references a parent that is still removed (`errors.<column>`, for example `errors.organizationId`); restore the parent first |
| 403 | Catalog data or users restored by a non-administrator (same rule as removal) |
| 404 | Any id is unknown, still active, or not visible to the caller; nothing is restored |
| 409 | The restored user's login is now used by another active account |

Access is checked exactly as for removal (members restore their project data, non-administrators only their own appointments). Restoring a user, project or assignment also restores the membership links (`UserProjects`, `UserAssignments`) that were removed together with it, as long as their other parent is active; links removed on their own stay removed. A member who removed a project can therefore restore it and keeps access to it.

### Self-service account

Any signed-in user (including a service client, which acts as its service account) manages their own account; the account always comes from the token's `sub`, never from the request body.

| Method | Route | Description |
|--------|-------|-------------|
| `GET` | `/api/me` | `{ "id", "name", "login", "createdAt", "updatedAt" }`; credentials are never returned |
| `PATCH` | `/api/me` | Body `{ "name" }`: changes only the display name (domain name rules, 400 `errors.name`). Returns `{ "success": true }` |
| `POST` | `/api/me/password` | Body `{ "currentPassword", "newPassword" }`. Returns `{ "success": true }` |

A wrong current password is 400 with `errors.currentPassword` (`"The current password is incorrect."`); the new password follows the same policy as user administration (`errors.newPassword`) and must differ from the current one. Five wrong current passwords for one account within 15 minutes lock password changes for that account for 15 minutes: 429 with `Retry-After` and the error envelope, even for the right password. At most four password changes are verified at once. A successful change replaces the Argon2id hash, so the security stamp changes: every existing access token, refresh token and identity cookie of the account stops working (within the 30-second session cache) and the client signs in again with the new password. A token whose subject has no active account is rejected with 401 by session validation; without a subject the API answers 401 as well.

### Error responses

Every non-2xx JSON response from WebApi and IdentityApi uses one envelope:

```json
{
  "statusCode": 400,
  "message": "The request is invalid.",
  "errors": { "endDate": ["End date must be on or after start date."] }
}
```

Validation failures are built by the same envelope helper as every other error (`application/json`, the shared default message). A field missing from a JSON body is reported under its own key by the validator, never as a serializer error. Error responses keep the security headers. Conditional requests (ETag / `If-None-Match`) are validated per caller: the ETag includes the caller's identity, so one account's cached list is never revalidated for another. Live listings revalidate the caller's session before every snapshot and close when it is no longer valid.

`errors` is present for validation and domain-rule failures; keys are camelCase request property names, and `generalErrors` holds messages not tied to one field. Statuses: 400 validation/domain rule or a reference to a missing or removed record, 401 missing/invalid/revoked token, 403 not allowed (for example changing catalog data, someone else's appointment, or moving a row into a project you don't belong to), 404 not found or not visible (also for writes to rows you can't see, on every persistence path), 409 duplicate id or login, stale project version, active dependents, or a concurrent change to the same rows (retry), 429 rate limited (with `Retry-After`), 500 unexpected error (details are only logged). gRPC maps the same cases to `InvalidArgument`, `Unauthenticated`, `PermissionDenied`, `AlreadyExists` and `FailedPrecondition`, or to `Success=false` results for missing rows and stale versions.

### Dates, search and sorting

Timestamps are stored as UTC. Values with an offset are converted; values without one (for example `"2064-06-09"`) are treated as UTC. `search` matches literally: `%`, `_` and `\` are not wildcards. `sortColumn` accepts persisted column names case-insensitively (credential columns are never sort keys) and falls back to `Id`; `sortOrder` is `ASC` or `DESC`.

### Available Entities

| Entity | Singular route | List route |
|--------|----------------|------------|
| Appointment | `/api/appointment` | `/api/appointments` |
| Assignment | `/api/assignment` | `/api/assignments` |
| AssignmentImpediment | `/api/assignmentImpediment` | `/api/assignmentImpediments` |
| AssignmentType | `/api/assignmentType` | `/api/assignmentTypes` |
| Impediment | `/api/impediment` | `/api/impediments` |
| Organization | `/api/organization` | `/api/organizations` |
| Project | `/api/project` | `/api/projects` |
| User | `/api/user` | `/api/users` |
| UserAssignment | `/api/userAssignment` | `/api/userAssignments` |
| UserProject | `/api/userProject` | `/api/userProjects` |
| Workflow | `/api/workflow` | `/api/workflows` |

### Example: Create Appointment

**Request:** replace the example `assignmentId` and `userId` with existing related IDs. A supplied nonempty `id` is preserved; omitting it lets the entity factory generate one.

```http
POST /api/appointment
Authorization: Bearer <token>
Content-Type: application/json

{
  "id": "67d29a03-9200-4d6e-9030-009f4b060ce9",
  "description": "Sprint planning meeting",
  "keepDate": "2026-09-18T10:00:00Z",
  "amountHours": 2,
  "assignmentId": "35f1a233-e070-4205-909d-0eaabf89aec4",
  "userId": "35b9c5c1-6abf-4d50-aee8-00abe2f09560"
}
```

**Response (200 OK, abbreviated):**

```json
{
  "appointment": {
    "id": "...",
    "description": "Sprint planning meeting",
    "keepDate": "2026-09-18T10:00:00Z",
    "amountHours": 2,
    "assignmentId": "...",
    "userId": "...",
    "createdAt": "...",
    "active": true
  }
}
```

### Data Access

REST deliberately mixes persistence examples. Many writes use EF Core through `IApplicationDbContext`; most reads use generic Dapper through `IUnitOfWork`. Organizations use generic Dapper, projects use `IProjectRepository`, and project creation shares an Application handler with gRPC. Impediments remain an EF-backed CRUD example. Inspect the endpoint constructor for the specific dependency.

### WebClient response expectations

The WebClient CRUD screens consume the REST endpoints through `src/WebClient/src/lib/api/webapi-client.ts` and normalize the response shapes used by the API:

- list arrays, paginated objects, and `{ result: ... }` list envelopes
- raw singular items with an `id`
- `{ result: item }` singular envelopes
- resource-key singular envelopes such as `{ organization: { ... } }`

Singular normalization is used to reload a project's current version after an edit conflict. List requests use flat query keys only (`pageNumber`, `pageSize`, `search`, `ids`) plus `sortColumn=CreatedAt&sortOrder=ASC`. Missing relation labels use batched list requests with comma-separated `ids` (at most 100 per request), then merge into the existing cache. Selected relations keep their cached labels across search pages. Listings open the SSE stream directly and use its first event as the initial snapshot. See [WebClient CRUD](../webclient-crud/) for error, authorization and reconnect handling.

### Rate Limiting

- 300 requests per minute per IP address (a dashboard and CRUD forms issue several requests per screen); `/healthz` and `/readyz` are exempt
- Fixed-window partitioning
- Queue limit: 20 additional requests
- Returns `429 Too Many Requests` with the error envelope and a `Retry-After` value derived from the limiter lease (60 seconds if metadata is unavailable); CORS exposes `Retry-After` to the browser client

The partition key is `HttpContext.Connection.RemoteIpAddress`; this is per process, not a distributed quota. Production sets `ASPNETCORE_FORWARDEDHEADERS_ENABLED=true` on the API containers so the address is the client behind Traefik and NGINX.

---

## IdentityApi -- Authentication

### Base URL

`http://localhost:5200`

### Swagger

Available at `/swagger`, currently enabled in all environments.

### OpenID Connect provider

IdentityApi is an OpenID Connect provider built on [Open.IdentityServer](https://github.com/RockSolidKnowledge/Open.IdentityServer) (Apache 2.0, the continuation of IdentityServer4). The protocol endpoints are listed in `/.well-known/openid-configuration`:

| Endpoint | Route | Use |
|----------|-------|-----|
| Discovery | `/.well-known/openid-configuration` | Issuer, endpoints, grant types |
| Signing keys (JWKS) | `/.well-known/openid-configuration/jwks` | Public RS256 keys the API hosts validate with |
| Authorize | `/connect/authorize` | Authorization code flow with PKCE (S256) |
| Token | `/connect/token` | `authorization_code`, `refresh_token`, `client_credentials` |
| Revocation | `/connect/revocation` | Revokes a refresh token (RFC 7009) |
| Introspection | `/connect/introspect` | Token introspection |
| End session | `/connect/endsession` | Signs out of the identity session |
| User info | `/connect/userinfo` | Profile claims |

The device flow is disabled. The `implicit` grant appears in discovery because the framework supports it, but no client may use it.

#### Clients

| Client | Flow | Notes |
|--------|------|-------|
| `cpnucleo-webclient` | Authorization code + PKCE, no secret | Scopes `openid profile cpnucleo.api offline_access`; redirect `{origin}/signin-callback/` and post-logout `{origin}/login/` for each `Cors__AllowedOrigins__N` |
| `Identity__ServiceClients__{id}` | Client credentials | Optional. `Secret` (32+ characters), `Login` (the service account) and `Scopes` (default `cpnucleo.api cpnucleo.grpc`) |

A service client's token carries its service account's subject, login, admin claim and security stamp, so the API hosts apply the same rules as for people: deactivating the account or changing its credentials revokes the client. A client whose account isn't active gets no token.

#### Sign-in and sign-out steps

The sign-in page is the WebClient's Astro page; IdentityApi only adds four anonymous endpoints:

| Method | Route | Description |
|--------|-------|-------------|
| `GET` | `/api/account/login-page?authRequest=` | The server's login URL. Re-validates the pending authorization request and redirects to `{origin}/login/?authRequest=<absolute request URL>` on the WebClient origin of its `redirect_uri`, when that origin is one of `Cors__AllowedOrigins__N`. Any other origin gets the first configured origin's page (or `Identity__LoginPageUrl` when set); a missing or invalid request goes there with `error=request` |
| `POST` | `/api/account/login` | Form post (`login`, `password`, `authRequest`) from the WebClient sign-in page. Redirects to the pending authorization request, or back to the sign-in page with `error=invalid`, `error=locked&retryAfter=N` or `error=request` |
| `GET` | `/api/account/logout?logoutId=` | The end-session endpoint's logout step: ends the server-side session, removes the identity cookie, redirects to the post-logout URL |
| `GET` | `/api/account/error?errorId=` | `{ "error", "errorDescription" }` for an authorization request the server rejected |

The sign-in post must come from a WebClient origin (`Origin`, or `Referer`); others get 403 with the error envelope, and sign-in errors return to the sign-in page of the origin that posted. It only returns to a pending authorization request on this host. Unknown, ambiguous (legacy duplicates) and wrong credentials take the same Argon2id time and redirect identically. Five failures for one login within 15 minutes lock it for 15 minutes regardless of client address; at most four password verifications run at once. `Login` is limited to 256 characters and `Password` to 128.

### Tokens

| Parameter | Value |
|-----------|-------|
| Issuer | `https://identity-cpnucleo.jonathanperis.tech` (`Jwt__Issuer`) |
| Audiences | WebApi: `https://api-cpnucleo.jonathanperis.tech` (scope `cpnucleo.api`); GrpcServer: `https://grpc-cpnucleo.jonathanperis.tech` (scope `cpnucleo.grpc`) |
| Access token | RS256 JWT, `typ: at+jwt`, 30 minutes |
| Refresh token | One-time use; each refresh returns a successor. Never valid past eight hours from sign-in |
| Identity cookie | Absolute eight hours, not sliding, `SameSite=Lax` |

Access tokens carry `sub`, `sid` (the sign-in session), `cpnucleo:login`, `cpnucleo:security_stamp` and, for administrators, `cpnucleo:admin`. The claims are recomputed from the account at every issuance, refreshes included.

#### Signing keys

IdentityApi signs with a rotating RSA key ring stored in `IdentitySigningKeys`. Each key signs for 90 days; its successor is published one day earlier, and a replaced key stays published for seven more days. Private keys are encrypted at rest with AES-256-GCM under a key derived from `Jwt__SigningKey` (or `Identity__KeyEncryptionSecret`). Data Protection keys (identity cookies) live encrypted in `IdentityDataProtectionKeys`, so restarts don't sign anyone out. `Jwt__SigningPrivateKey` (PEM) pins one externally managed key instead.

#### Validation in the API hosts

WebApi and GrpcServer read the keys from the discovery document (`Jwt__MetadataAddress`, defaulting to the issuer's public address) and refresh them when a token names an unknown `kid`. They pin RS256, `at+jwt`, the issuer, their own audience and the lifetime. Raw `sub` is retained (`MapInboundClaims = false`).

After signature validation they confirm, with a 30-second cache, that the account is active, the security stamp still matches, an admin claim is still listed in `CPNUCLEO_ADMIN_LOGINS`, and the token's sign-in session (`sid`) hasn't ended. A password change, deactivation, admin removal, sign-out or refresh-token replay therefore takes effect within 30 seconds.

#### Sessions and revocation

| Event | Effect |
|-------|--------|
| Sign-out (end session) | Session ended, its refresh tokens deleted, identity cookie removed, its access tokens rejected by the APIs |
| Refresh token replayed | Treated as theft: the whole session ends as above |
| Revocation endpoint | The given refresh token stops working |
| Password or login change (administrator or `POST /api/me/password`) | The identity cookie and refresh tokens stop working (security stamp); access tokens are rejected |
| Eight hours after sign-in | Refresh tokens and the identity cookie expire |

### Rate Limiting

- 60 requests per minute per IP address, queue limit 5. A sign-in takes four requests and every tab refreshes on its own.
- Brute force is bounded per login by the lockout and the sign-in concurrency cap described above.

---

## GrpcServer -- Remote Command Handling

The GrpcServer uses FastEndpoints.Messaging.Remote to handle commands over HTTP/2 gRPC transport.

### Ports

- gRPC transport: `http://localhost:5300` (HTTP/2)
- Health check: `http://localhost:5301/healthz` (HTTP/1.1)

### Command Pattern

Each operation is a command/result pair defined in `GrpcServer.Contracts`:

```
Command -> Handler -> Result
```

### Available Commands (per entity)

Each of the 11 entities has these 5 commands:

| Command | Description |
|---------|-------------|
| `Create{Entity}Command` | Create a new record |
| `Get{Entity}ByIdCommand` | Retrieve by ID |
| `List{Entity}sCommand` | List with pagination |
| `Remove{Entity}Command` | Soft delete |
| `Update{Entity}Command` | Update fields |

Total: 55 registered command handlers. Restore (`POST /api/{entity}/restore`) and the self-service account endpoints are REST-only; list commands accept the same relation and date filters as REST.

### Data Access

gRPC persistence uses Dapper. Most handlers use `IUnitOfWork` with explicit transaction operations; project creation delegates to the shared Application handler and its `IProjectCreateStore` port. The transport uses typed .NET commands/results rather than a hand-maintained public `.proto` API.

### Handler Registration

Handlers are registered in `Program.cs`:

```csharp
app.MapHandlers(h =>
{
    h.Register<CreateAppointmentCommand, CreateAppointmentHandler, CreateAppointmentResult>();
    h.Register<GetAppointmentByIdCommand, GetAppointmentByIdHandler, GetAppointmentByIdResult>();
    // ... 53 more handlers
});
```

---

## Health Checks

All three APIs expose separate liveness and readiness endpoints:

| Service | URL | Protocol |
|---------|-----|----------|
| WebApi | `/healthz`, `/readyz` | HTTP; lab port 5100 |
| IdentityApi | `/healthz`, `/readyz` | HTTP; lab port 5200 |
| GrpcServer | `/healthz`, `/readyz` | HTTP/1.1 diagnostics; lab port 5301 |

### Root Endpoint

The API hosts map `GET /` to `"Hello World!"`; the gRPC host's fallback authorization policy applies to that root route. The WebClient root serves the application HTML.

---

## Authentication Flow

JWT authentication is enforced by WebApi and GrpcServer. User administration additionally requires the configured administrator claim on both transports:

1. The WebClient starts the authorization code flow with PKCE at IdentityApi's `/connect/authorize`.
2. Without a session, IdentityApi sends the browser to the WebClient sign-in page, which posts the credentials to `/api/account/login`.
3. IdentityApi issues an authorization code to `{origin}/signin-callback/`; the WebClient redeems it at `/connect/token` with the PKCE verifier and gets an access token, a one-time refresh token and an identity token.
4. The access token goes in `Authorization: Bearer {token}` for WebApi; service clients use `client_credentials` for WebApi or GrpcServer.
5. The APIs validate signature (JWKS), issuer, audience, type and lifetime, then the account, stamp, admin claim and session.

Refreshes use the `refresh_token` grant; signing out uses `/connect/revocation` and `/connect/endsession`.

## Query and update contracts

List query fields are flat: `pageNumber`, `pageSize` (1–100), `sortColumn`, `sortOrder`, `search` (up to 128 characters), and `ids` (up to 100 comma-separated UUIDs). For example: `/api/projects?pageSize=25&search=school`. FastEndpoints binds these scalar fields into the request's `Pagination` object; dotted `pagination.*` keys are not the canonical HTTP contract. Out-of-bound values are a 400 with the offending field (`errors.pageSize`, `errors.ids`, ...). gRPC list commands apply the same rules and answer `InvalidArgument` with the same message, including for hand-crafted messages that bypass the typed contracts or omit `Pagination`.

Lists also accept optional relation and date filters, combined with `AND` on top of visibility, `search` and `ids` (they only ever narrow what the caller may see):

| Field | Value | Applies to |
|-------|-------|------------|
| `organizationId` | One UUID | Projects |
| `projectId` | One UUID | Assignments, UserProjects |
| `assignmentId` | One UUID | Appointments, AssignmentImpediments, UserAssignments |
| `userId` | One UUID | Assignments, Appointments, UserAssignments, UserProjects |
| `workflowId` | One UUID | Assignments |
| `dateFrom`, `dateTo` | ISO-8601 date or date-time (`2031-01-20`, `2031-01-20T10:00:00Z`, `2031-01-20T10:00:00%2B02:00`); values without an offset are UTC | Appointments: `keepDate` in `[dateFrom, dateTo)`. Assignments: the `startDate`..`endDate` period overlaps the range (`endDate >= dateFrom` and `startDate < dateTo`) |

Each bound is optional. A malformed UUID is 400 `errors.<field>` (`"ProjectId must be a UUID."`), a malformed date `"DateFrom must be an ISO-8601 date and time."`, and `dateFrom` after `dateTo` is reported on `errors.dateTo`. A filter the resource does not support is rejected rather than ignored: 400 with the field (`"Projects cannot be filtered by userId."`), and gRPC `InvalidArgument` with the same message. Live listings (`Accept: text/event-stream`) apply the same filters to every snapshot and reject the same requests with 400 before the stream starts. Encode `+` in offsets as `%2B`.

Project PATCH requests may include `expectedVersion`, using the last observed `updatedAt` or initial `createdAt`. A stale value returns HTTP 409. The corresponding gRPC command accepts `ExpectedVersion` and reports a failed result on a conflict. Omitting the field preserves legacy last-write-wins behavior.

All normal removal paths soft-delete, and `POST /api/{entity}/restore` undoes them. Project batches are atomic. The database rejects conflicting normalized active logins on new/changed accounts; authentication rejects ambiguous legacy logins rather than choosing an arbitrary account.

`/healthz` is liveness only. `/readyz` checks database/schema availability. SSE listings refresh periodically so writes through other instances/transports converge within a refresh cycle.

## Source of truth

- [REST endpoints and request DTOs](https://github.com/jonathanperis/cpnucleo/tree/main/src/WebApi/Endpoints)
- [Identity endpoints](https://github.com/jonathanperis/cpnucleo/tree/main/src/IdentityApi/Endpoints)
- [gRPC command contracts](https://github.com/jonathanperis/cpnucleo/tree/main/src/GrpcServer.Contracts/Commands)
- [HTTP CRUD contract tests](https://github.com/jonathanperis/cpnucleo/blob/main/tests/WebApi.Integration.Tests/CrudContractTests.cs)
