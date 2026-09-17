# API Reference

Cpnucleo exposes three API services: the WebApi (REST), the IdentityApi (authentication), and the GrpcServer (gRPC command handling).

---

## WebApi -- REST Endpoints

The WebApi uses [FastEndpoints](https://fast-endpoints.com/) to define REST endpoints with Swagger/OpenAPI documentation. Each entity has 5 standard CRUD endpoints.

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

These routes do not contain an `/{id}` path segment. PATCH uses each resource's request DTO, not JSON Patch operations. Include `Authorization: Bearer <token>` on CRUD requests. All user-resource operations additionally require the administrator claim.

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

Singular normalization supports edit/detail loads. Missing relation labels use batched list requests with comma-separated `ids`, then merge into the existing cache. Selected relations remain available across search pages.

### Rate Limiting

- 50 requests per minute per IP address
- Fixed-window partitioning
- Queue limit: 10 additional requests
- Returns `429 Too Many Requests` with a `Retry-After` value derived from the limiter lease (60 seconds if metadata is unavailable)

The partition key is `HttpContext.Connection.RemoteIpAddress`; this is per process, not a distributed quota. A reverse proxy may affect which address the application sees.

---

## IdentityApi -- Authentication

### Base URL

`http://localhost:5200`

### Swagger

Available at `/swagger`, currently enabled in all environments.

### Login Endpoint

| Method | Route | Description |
|--------|-------|-------------|
| `POST` | `/api/login` | Authenticate and receive JWT token |

**Request:**

```http
POST /api/login
Content-Type: application/json

{
  "login": "user@example.com",
  "password": "password123"
}
```

**Response (200 OK):**

```json
{
  "token": "eyJhbGciOiJIUzI1NiIs..."
}
```

**Response (404 Not Found):**

Returned when credentials are invalid.

### JWT Configuration

| Parameter | Value |
|-----------|-------|
| Issuer | `https://identity-cpnucleo.jonathanperis.tech` |
| Audience | `https://api-cpnucleo.jonathanperis.tech` |
| Access-token lifetime | 30 minutes; refresh is capped by the original eight-hour session |
| Algorithm | HMAC-SHA (via FastEndpoints.Security) |

Issuer and audience above are the checked-in defaults. All API hosts must use matching `Jwt__Issuer`, `Jwt__Audience`, and `Jwt__SigningKey` configuration. Raw `sub` claims are retained during validation.

### Refresh Endpoint

`POST /api/refresh` accepts a valid bearer token and no request body, returning the same `{ "token": "..." }` envelope. It returns 401 for an inactive/missing account or a session outside the eight-hour boundary. It recalculates admin privileges from `CPNUCLEO_ADMIN_LOGINS`; legacy tokens without the session-start claim require a new login. There is no separate long-lived refresh token.

### Rate Limiting

- 10 requests per minute per IP address
- Queue limit: 5 additional requests
- Stricter than WebApi to protect against brute-force attacks

### Output Caching

The host registers a 10-second base output-cache policy. ASP.NET Core's default eligibility rules still apply; this is not a promise to cache every response, and POST login/refresh responses are not cached by that default policy.

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

Total: 55 registered command handlers.

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

1. Client authenticates via `POST /api/login` on IdentityApi
2. Receives JWT token
3. Includes token in `Authorization: Bearer {token}` header for WebApi/GrpcServer requests
4. Token validation checks issuer, audience, signing key, and expiration

Access tokens last up to 30 minutes. `POST /api/refresh` requires an active account and an original session younger than eight hours; it recalculates admin privileges. Tokens created before the session-start claim was introduced require a fresh login when refreshing.

## Query and update contracts

List query fields are flat: `pageNumber`, `pageSize` (1–100), `sortColumn`, `sortOrder`, `search` (up to 128 characters), and `ids` (up to 100 comma-separated UUIDs). For example: `/api/projects?pageSize=25&search=school`. FastEndpoints binds these scalar fields into the request's `Pagination` object; dotted `pagination.*` keys are not the canonical HTTP contract.

Project PATCH requests may include `expectedVersion`, using the last observed `updatedAt` or initial `createdAt`. A stale value returns HTTP 409. The corresponding gRPC command accepts `ExpectedVersion` and reports a failed result on a conflict. Omitting the field preserves legacy last-write-wins behavior.

All normal removal paths soft-delete. Project batches are atomic. The database rejects conflicting normalized active logins on new/changed accounts; authentication rejects ambiguous legacy logins rather than choosing an arbitrary account.

`/healthz` is liveness only. `/readyz` checks database/schema availability. SSE listings refresh periodically so writes through other instances/transports converge within a refresh cycle.

## Source of truth

- [REST endpoints and request DTOs](https://github.com/jonathanperis/cpnucleo/tree/main/src/WebApi/Endpoints)
- [Identity endpoints](https://github.com/jonathanperis/cpnucleo/tree/main/src/IdentityApi/Endpoints)
- [gRPC command contracts](https://github.com/jonathanperis/cpnucleo/tree/main/src/GrpcServer.Contracts/Commands)
- [HTTP CRUD contract tests](https://github.com/jonathanperis/cpnucleo/blob/main/tests/WebApi.Integration.Tests/CrudContractTests.cs)
