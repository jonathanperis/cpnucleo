# API Reference

Cpnucleo exposes three API services: the WebApi (REST), the IdentityApi (authentication), and the GrpcServer (gRPC command handling).

---

## WebApi -- REST Endpoints

The WebApi uses [FastEndpoints](https://fast-endpoints.com/) to define REST endpoints with Swagger/OpenAPI documentation. Each entity has 5 standard CRUD endpoints.

### Base URL

- Direct: `http://localhost:5100`
- Via NGINX load balancer: `http://localhost:9999`

### Swagger

Available in Development mode at `/swagger`.

### Endpoint Pattern

Every entity follows this consistent pattern:

| Method | Route | Description |
|--------|-------|-------------|
| `POST` | `/api/{entity}` | Create a new record |
| `GET` | `/api/{entity}/{id}` | Get a single record by ID |
| `GET` | `/api/{entity}` | List records (paginated) |
| `PUT` | `/api/{entity}/{id}` | Update an existing record |
| `DELETE` | `/api/{entity}/{id}` | Remove a record (soft delete) |

### Available Entities

| Entity | Route Prefix | Tag |
|--------|-------------|-----|
| Appointment | `/api/appointment` | Appointments |
| Assignment | `/api/assignment` | Assignments |
| AssignmentImpediment | `/api/assignmentimpediment` | AssignmentImpediments |
| AssignmentType | `/api/assignmenttype` | AssignmentTypes |
| Impediment | `/api/impediment` | Impediments |
| Organization | `/api/organization` | Organizations |
| Project | `/api/project` | Projects |
| User | `/api/user` | Users |
| UserAssignment | `/api/userassignment` | UserAssignments |
| UserProject | `/api/userproject` | UserProjects |
| Workflow | `/api/workflow` | Workflows |

### Example: Create Appointment

**Request:**

```http
POST /api/appointment
Content-Type: application/json

{
  "id": "00000000-0000-0000-0000-000000000000",
  "description": "Sprint planning meeting",
  "keepDate": "2025-03-01T10:00:00Z",
  "amountHours": 2,
  "assignmentId": "...",
  "userId": "..."
}
```

**Response (200 OK):**

```json
{
  "appointment": {
    "id": "...",
    "description": "Sprint planning meeting",
    "keepDate": "2025-03-01T10:00:00Z",
    "amountHours": 2,
    "assignmentId": "...",
    "userId": "...",
    "createdAt": "...",
    "active": true
  }
}
```

### Data Access

All WebApi endpoints use EF Core via `IApplicationDbContext` for database operations. Endpoints inject the context directly:

```csharp
public class Endpoint(IApplicationDbContext dbContext) : Endpoint<Request, Response>
```

### Rate Limiting

- 50 requests per minute per IP address
- Fixed-window partitioning
- Queue limit: 10 additional requests
- Returns `429 Too Many Requests` with `Retry-After: 60` header when exceeded

### API Client Generation

The WebApi generates downloadable API clients via Kiota:

- **C# client**: Available at `/cs-client`
- **TypeScript client**: Generated during build

---

## IdentityApi -- Authentication

### Base URL

`http://localhost:5200`

### Swagger

Available in Development mode at `/swagger`.

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
| Issuer | `https://identity.peris-studio.dev` |
| Audience | `https://peris-studio.dev` |
| Expiration | 1 day |
| Algorithm | HMAC-SHA (via FastEndpoints.Security) |

### Rate Limiting

- 10 requests per minute per IP address
- Queue limit: 3 additional requests
- Stricter than WebApi to protect against brute-force attacks

### Output Caching

- Base policy: 10-second cache expiration
- All responses are cached by default

---

## GrpcServer -- Remote Command Handling

The GrpcServer uses FastEndpoints.Messaging.Remote to handle commands over HTTP/2 gRPC transport.

### Ports

- Health check: `http://localhost:5300/healthz` (HTTP/1.1)
- gRPC transport: `http://localhost:5301` (HTTP/2)

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

All gRPC handlers use Dapper via `IUnitOfWork` for database operations, providing transactional support with explicit `BeginTransactionAsync`, `CommitAsync`, and `RollbackAsync`.

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

All three APIs expose health check endpoints:

| Service | URL | Protocol |
|---------|-----|----------|
| WebApi | `/healthz` | HTTP |
| IdentityApi | `/healthz` | HTTP |
| GrpcServer | `/healthz` | HTTP |

### Root Endpoint

All services also respond to `GET /` with `"Hello World!"`.

---

## Authentication Flow

JWT authentication is configured but currently commented out in WebApi and GrpcServer. The IdentityApi is fully functional for token generation. When enabled:

1. Client authenticates via `POST /api/login` on IdentityApi
2. Receives JWT token
3. Includes token in `Authorization: Bearer {token}` header for WebApi/GrpcServer requests
4. Token validation checks issuer, audience, signing key, and expiration
