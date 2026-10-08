# FastEndpoints adoption map (October 2026)

A page-by-page comparison of the [FastEndpoints documentation](https://fast-endpoints.com/docs/get-started) with how Cpnucleo uses FastEndpoints 8.3.0. It lists what is already standard, what we can adopt, and what we deliberately keep because a project rule in `AGENTS.md` takes precedence.

**Scope:** every page in the docs sidebar (Get Started through Native AOT), every cookbook recipe relevant to a CRUD API, plus the AI-agent and x402 guides.

**Hosts covered:**
- WebApi: 69 endpoints in `src/WebApi/Endpoints/**`.
- GrpcServer: 55 Remote Messaging handlers.
- IdentityApi, where it shares configuration with WebApi.
- The WebApi unit and integration test projects.

**How it was checked:**
- **Source:** counts come from grepping the source.
- **Behaviour:** checked in a scratch FastEndpoints 8.3.0 app and on a local Debug build of WebApi.
- **Production:** the OpenAPI and anonymous-request findings were re-checked against the production API on 2026-10-08.
- **Status:** item 1 (the defect below) is fixed together with this document; the other items are implemented in follow-up pull requests.

## Implementation status

The backlog below was implemented in five stacked pull requests:

| PR | Items | Result |
|---|---|---|
| #271 | 1 | Delta runs only for signed-in callers; anonymous `/`, `/swagger` and the OpenAPI document are served again. |
| #272 | 2, 3, 4, 14 | No `required` request members (architecture test); validation errors built by `ApiErrors.ValidationResponse` through `ErrorOptions`; senders mark the response started; duplicate ids are 409. |
| #273 | 6–11 | One tag per operation with descriptions, readable operation ids and schema names, documented error statuses and flat pagination keys, and `docs/openapi` snapshots checked by `OpenApiSnapshotTests`. |
| #274 | 5, 13, 15, 16 | Constructor-injected notifier, FastEndpoints event-stream sender, boilerplate logs removed, `UserAdministrationGroup`. |
| #275 | 12, 17, 18 | Source-generated discovery and `Warmup()`; `[Authorize]` on gRPC user handlers with the denial message kept; per-call gRPC timing; `FastEndpoints.Testing` replaced by `Microsoft.AspNetCore.Mvc.Testing`. |

Decisions taken while implementing:

- **#17** keeps the client-facing message. A gRPC-aware authorization result handler answers a forbidden call with `PermissionDenied` and "User administration requires an administrator.", so moving to `[Authorize]` changed nothing for clients.
- **#18** drops `FastEndpoints.Testing`. Its `AppFixture` is single-host, and the suite deliberately runs IdentityApi, WebApi and GrpcServer together with real tokens.
- **#19 is not adopted.** FastEndpoints keeps event handlers in a process-wide static dictionary and creates them through a process-wide service resolver. The integration tests host several APIs in one process, so a listing event handler would resolve its dependencies from whichever host started first, and the tests would no longer exercise what production runs. #5 already removed the service-locator calls the event bus would have replaced.
- **#11** compares the document served by each host in its own process. A document generated inside the shared test process differs, for the same reason as below.

**Finding from the implementation:** FastEndpoints applies `UseFastEndpoints(...)` settings once per process, and the integration fixture starts IdentityApi first. Until #273, WebApi in tests silently ran with IdentityApi's settings; it only worked because they matched. The block is now identical in both hosts, pinned by `ApiHosts_ShouldConfigureFastEndpointsIdentically`, and per-host OpenAPI behaviour lives in NSwag processors.

## Verdict summary

| Verdict | Meaning | Count |
|---|---|---|
| Already standard | Matches the documented practice, or an equivalent that is at least as strict | most rows in the matrix |
| Adopt | Contract-safe improvement; listed in the backlog below | 19 items |
| Lab only | Worth doing only as an isolated, labelled experiment | 5 |
| Keep ours | A documented feature conflicts with a project rule | 18 |
| Not applicable | Feature unrelated to this API (files, XML, cookies, payments, ...) | remaining rows |

The fundamentals are already on standard:
- Feature-folder endpoints (`Endpoint.cs` + `Models.cs`), constructor injection and nested `Validator<T>` classes.
- Secure-by-default endpoints with one named policy.
- ASP.NET JWT bearer validation against IdentityApi's JWKS.
- The ASP.NET rate limiter wired through `Options(x => x.RequireRateLimiting(...))`.
- `Factory.Create<TEndpoint>` unit tests.
- Remote Messaging with a contracts project.
- The `CreateSlimBuilder` host.

The gaps sit in four places:
- the error and OpenAPI metadata layer;
- one serializer behaviour that leaks internal type names;
- service-locator calls inside endpoints;
- tooling we reference but don't use.

## Defect found during the review

**Anonymous `GET /`, `/swagger/index.html` and `/swagger/v1/swagger.json` returned 500 on WebApi, in production too.** Fixed in #271.

- **Cause:** `UseInfrastructure()` runs Delta (ETag caching) with a per-caller `suffix` callback (`src/Infrastructure/DependencyInjection.cs:49-58`). Delta throws "A suffix callback was provided but the user is not authenticated" for any anonymous GET that reaches it.
- **Not affected:**
  - `/api/*` routes reject anonymous callers with 401 before Delta runs.
  - `/healthz` and `/readyz` are mapped earlier in the pipeline. That is why the release smoke tests pass.
- **Fix:** add `httpContext.User.Identity?.IsAuthenticated == true` to Delta's `shouldExecute`.
- **Test:** add an integration test for an anonymous `GET /swagger/v1/swagger.json` that expects 200. `docs/wiki/api-reference.md` advertises `/swagger`.

## Adoption backlog

Ordered by value and risk. "Contract" means a client could observe the change; every such item names the tests that pin today's behaviour.

### Tier 1: correctness of the HTTP contract

| # | Change | Docs | Today | Effort | Contract and tests |
|---|---|---|---|---|---|
| 1 | Fix the Delta anonymous 500 (above). | — | Swagger UI is unusable for anonymous visitors. | S | No contract change; add the integration test. |
| 2 | **Drop `required` from JSON-body request DTOs.** Use `= string.Empty` / `= []` defaults and let the existing validators reject empty values. Add an architecture test like the existing response-DTO rule (`WebApiEndpointSourceTests.cs`). Exclude `[FromQuery] PaginationParams`. | Model binding: "Mixed binding sources & the `required` keyword" | 18 body `Models.cs` files plus `Common/Models/RemoveRequest.cs` and `RestoreRequest.cs`. A missing property makes System.Text.Json fail before validation. The 400 then carries `errors.serializerErrors: ["JSON deserialization for type 'WebApi.Endpoints....Request' was missing required properties ..."]`, not `errors.name`. | S–M | Yes, an improvement: field errors instead of a leaked type name. The current message breaks the rule that only domain and access messages reach clients. Add an integration test that omits a field. |
| 3 | **Make validation 400s the same envelope by construction.** Set `c.Errors.ResponseBuilder` to return `ApiErrorResponse` (camelCase field keys, `ApiErrors.GeneralErrorsKey`), `c.Errors.ContentType = "application/json"` and `c.Errors.ProducesMetadataType = typeof(ApiErrorResponse)`. | Configuration settings: error options; Cookbook: custom error responses | FastEndpoints' default `ErrorResponse` happens to have the same fields. It says "One or more errors occurred!", while `ApiErrors.DefaultMessage(400)` says "The request is invalid.". OpenAPI declares it as `application/problem+json`. | S | `message` changes text, and the WebClient shows it (`http-client.ts`; mocked in `http-client.test.ts:123`). Field keys pinned by `ApiContractTests`. Loosen the exact-string pin at `FastEndpointsConfigurationTests.cs:516`. |
| 4 | **Finish the custom send methods.** `NotFoundEnvelopeAsync` must call `HttpContext.MarkResponseStart()`, as the docs require for custom senders. Add `ConflictEnvelopeAsync(message)` and `TooManyRequestsEnvelopeAsync(retryAfter, message)`. Route the 3 hand-written `ApiErrors.WriteAsync` calls (UpdateProject 409, ChangePassword 429/409) and IdentityApi's equivalents through them. | Misc conveniences: custom send methods | `src/WebApi/Common/Extensions/ResponseSenderExtensions.cs` and `src/IdentityApi/Common/ResponseSenderExtensions.cs` write the envelope without marking the response as started. | S | None. |
| 5 | **Inject `ListingChangeNotifier` through the constructor.** In tests use `Factory.Create<T>(ctx => ctx.AddTestServices(s => s.AddSingleton<ListingChangeNotifier>()), deps)` and delete the reflection helper `EndpointTestExtensions.WithListingServices()`. | Dependency injection; Testing: `AddTestServices` | 46 endpoints call `HttpContext.RequestServices.GetRequiredService<ListingChangeNotifier>()`. Every other dependency is constructor-injected. 50 test call sites swap `RequestServices` by reflection, which shares one notifier. | M (mechanical) | None. |

### Tier 2: OpenAPI accuracy (documentation only)

| # | Change | Docs | Today (verified on production) | Effort |
|---|---|---|---|---|
| 6 | `AutoTagPathSegmentIndex = 0` and key `TagDescriptions` by the plural tags, in both WebApi and IdentityApi. Optionally move each `WithTags` into one `c.Endpoints.Configurator` or a per-resource `Group` with an empty prefix. | OpenAPI: tags; Cookbook: endpoint grouping | Every operation has two tags (`Workflows`, `Workflow`). The document has 0 top-level tags, so none of the 12 descriptions in `Program.cs:141-155` appear. | S |
| 7 | Readable operation IDs: `c.Endpoints.NameGenerator = ctx => ctx.EndpointType.Namespace!.Split('.')[^1]`, giving `UpdateProject`. Make `SchemaNameGenerator` prefix `Request`/`Response` with the feature name (`UpdateProjectRequest`). | OpenAPI: endpoint names, schema names | `WebApiEndpointsWorkflowUpdateWorkflowEndpoint`; schemas `Request`, `Request2`, ..., `Response32`. `ShortNames` is unusable because every class is `Endpoint`. | S |
| 8 | `o.ExcludeNonFastEndpoints = true`. | OpenAPI | The minimal-API `GET /` "Hello World" appears as an untagged operation. | S |
| 9 | Declare the error responses that really happen: a global `Configurator` adding `Produces<ApiErrorResponse>(401/404/500, "application/json")`; per-endpoint 403 (User admin), 409 (versioned update, duplicates) and 429 (rate limiter, ChangePassword). Add `s.Responses[...]` text where behaviour is specific (atomic batch removal, version conflicts, `Retry-After`). | OpenAPI: default responses; Summary | Only 200, 400, 401 and 403 are documented. 404 is sent by 47 endpoints and is missing. | M |
| 10 | Document pagination as the flat query keys clients send (`pageNumber`, `pageSize`, `search`, `ids`, relation filters, `dateFrom`, `dateTo`), using an NSwag `IOperationProcessor`. Hide the computed `Offset` from the schema there, not with an attribute on the Domain type. | Cookbook: `IOperationProcessor` | List operations show one required `pagination` object parameter. | S–M |
| 11 | Export the OpenAPI document in CI (`ExportSwaggerDocsAndExitAsync("v1")` behind an `--export-openapi` mode) and diff it against a committed snapshot. This makes "preserve route paths and envelopes" executable. | OpenAPI: export; Cookbook: export at build time | No exported spec. | M |

### Tier 3: structure, startup and consistency

| # | Change | Docs | Today | Effort | Notes |
|---|---|---|---|---|---|
| 12 | Use the source generator we already reference: `o.SourceGeneratorDiscoveredTypes.AddRange(DiscoveredTypes.All)` instead of `o.Assemblies`, plus `c.Endpoints.Warmup()`. Optionally `c.Binding.ReflectionCache.AddFromWebApi()`. If we decide against it, remove `FastEndpoints.Generator` from `WebApi.csproj:42` and `IdentityApi.csproj:41`. | Configuration: source-generated discovery; Native AOT | Reflection discovery limited by `DisableAutoDiscovery` + `o.Assemblies` (`Program.cs:133-134`). The generated `DiscoveredTypes` is compiled and unused. | S–M | Keeps "only this host's endpoints". Confirm that the nested validators and `RemoveRequestValidator<T>` subclasses are discovered. A prerequisite for any Native AOT experiment. |
| 13 | Send listing streams with `Send.EventStreamAsync("listing", ListingSseExtensions.CreateListingStream(...), ct)`, or at least `Send.ResultAsync(TypedResults.ServerSentEvents(...))`. Move the 8-line branch repeated in 11 list endpoints into one helper. | Server-sent events; Misc: send methods | `TypedResults.ServerSentEvents(...).ExecuteAsync(HttpContext)` in 11 endpoints; FastEndpoints doesn't know the response started. | S | `EventStreamAsync` adds `X-Accel-Buffering: no`, event ids and shutdown cancellation (faster rolling restarts). It also writes `Connection: keep-alive` and an empty `retry:`. The WebClient parser reads only `data:` lines. Re-run `ConcurrencyAndStreamingTests` (token expiry, 15-second external-write convergence). |
| 14 | Replace `AddError` + `ThrowIfAnyErrors()` with `ThrowError(r => r.Prop, message, statusCode)`. Return 409 for a duplicate client-supplied `Id`, which the database path already does. | Validation: `ThrowError` | 12 endpoints. The pre-check returns 400, while the same race caught by the database returns 409. | S | Visible: 400 → 409 for duplicate ids. Unit tests expect `ValidationFailureException`, which `ThrowError` also throws. |
| 15 | Remove the boilerplate "Service started processing request." / "Service completed successfully." logs. Stop logging `request.Name` in Create endpoints (`User/CreateUser` logs a person's name). | Pre/post processors (global processors) | 126 boilerplate lines in 63 endpoints, plus 54 in gRPC handlers. `ElapsedTimeMiddleware` and OpenTelemetry already record start, end and duration. | S | Follows the spirit of "never log login names". |
| 16 | Group shared endpoint settings: a `Users` group carrying `Policies("UserAdministration")` and its tag. | Configuration: endpoint groups | `Policies("UserAdministration")` repeated in 6 endpoints. | S | Use an empty group prefix: routes mix `/user` and `/users` and must keep their paths. |
| 17 | GrpcServer: use `[Authorize(Policy = "UserAdministration")]` on the 5 User handlers' `ExecuteAsync`; the handler server copies those attributes into endpoint metadata. Move gRPC start/finish logging into the existing interceptor. | Remote procedure calls | `UserAdministration.RequireAdmin(IHttpContextAccessor)` inside each handler. | S | The status stays `PermissionDenied`, but the custom message "User administration requires an administrator." is lost. Decide whether that matters. |
| 18 | Testing package: either adopt `AppFixture<Program>` (Testcontainers in `PreSetupAsync`, cached hosts), or reference `Microsoft.AspNetCore.Mvc.Testing` directly and drop the unused `FastEndpoints.Testing`. | Integration & unit testing | Referenced and globally imported, 0 uses. A hand-written three-host `WebAppFixture` with one serial collection. | S (drop) / M–L (adopt) | `AppFixture` is single-host, while the suite deliberately runs IdentityApi, WebApi and GrpcServer together with real tokens. Running classes in parallel changes the deliberately serial "Database" collection; decide that explicitly. Recommendation: drop the package. |
| 19 | **Not adopted (see Implementation status).** Optional, after #5: publish an in-process `ListingChanged` event (`IEvent` + one `IEventHandler`) instead of calling the notifier. Assert writes with `RegisterTestEventReceivers()`. | Event bus | Direct notifier calls. | M | The 15-second external-write convergence stays the cross-instance guarantee. |

### Lab-only experiments

Each belongs under `labs/`, labelled as an experiment, never presented as a guarantee:
- **Job queues:** FastEndpoints job queues with a Dapper/PostgreSQL storage provider, next to `labs/OutboxLab`. Compare lease-based claiming with the hand-written `FOR UPDATE SKIP LOCKED` outbox. Queued jobs are not transactional with the business write unless the provider joins the transaction.
- **Native AOT:** source-generated serializer contexts and `NativeAotTestMode` black-box tests, as part of the explicit AOT experiment `AGENTS.md` requires.
- **`FastEndpoints.OpenApi`:** migrating from `FastEndpoints.Swagger` (NSwag) to it, which uses `Microsoft.AspNetCore.OpenApi`. The Swagger pins in `FastEndpointsConfigurationTests` and the `/swagger` docs would change.
- **Scalar:** Scalar as an alternative API reference UI (adds a package and CSP changes).
- **gRPC server streaming:** as the gRPC counterpart to REST SSE listings.

## Deliberately not adopted

| Documented feature | Why we keep our approach |
|---|---|
| Route parameters (`/projects/{id}`), API versioning, `CreatedAtAsync`/201 | `AGENTS.md`: preserve route paths and API envelopes. The WebClient and contract tests depend on `?id=`, singular/plural routes and 200 on create. Revisit only with a deliberate breaking release. |
| `c.Errors.UseProblemDetails()` | Replaces the single `statusCode/message/errors` envelope. |
| `UseDefaultExceptionHandler()` | Its `Reason` field exposes exception messages, which only `DomainException`/`AccessDeniedException` may do. `ApiExceptionMiddleware` is the documented "copy and adapt" route. |
| `ValidationContext.Instance.ThrowError` from deeper layers | Domain has no external packages, and its errors must work on both transports (`DomainException`). |
| Declarative `Claims`/`Roles`/`Permissions`, `AccessControl`, `[HasPermission]`, `IClaimsTransformation` permissions | Authorization lives in `ResourceAccess`, the Dapper repositories and `AccessGuardInterceptor`. A second model would diverge. |
| `FastEndpoints.Security` token creation, refresh tokens, revocation middleware | IdentityApi is the OpenID Connect provider and API hosts hold no keys. `TokenSessionValidator` in `OnTokenValidated` is stricter than the revocation middleware. |
| `Throttle()` | Header-spoofable, per instance and not in our envelope. The ASP.NET rate limiter is the documented alternative and is already used. |
| Response caching, output caching, idempotency middleware | Responses are per caller and access-filtered; a shared cache keyed by URL and body could replay another user's data. Creates are already idempotent through client-supplied ids, and batches are deduplicated. |
| FastEndpoints `Mapper<>` / `EndpointWithMapping` | Entities are built through domain factories; Mapperly handles entity-to-DTO mapping at compile time. |
| `ShortNames = true` | Every endpoint class is named `Endpoint` by convention, so names would collide. Use `NameGenerator` (#7). |
| Query collection binding (`List<Guid>` from `?ids=`) | `PaginationParams` setters must never throw and are shared with the gRPC MessagePack contract. `ids` stays a validated comma-separated string. |
| `[FromClaim]` caller binding | One `ICurrentUser` serves REST, gRPC and repositories. |
| Test ordering with `[Priority]` | The order-dependent suite was removed on purpose. |
| Mock authentication handler in tests | Tests use real RS256 tokens from IdentityApi so session validation and the access model are exercised. |
| Remote event hubs for cross-process listing updates | WebApi may not reference GrpcServer contracts. In-memory best-effort delivery can't replace the 15-second convergence guarantee. |
| `JsonPatchDocument`, OData, generic CRUD base endpoints | Would change request envelopes and bounded flat queries, and erase the EF Core and Dapper comparisons the lab exists to show. |
| `FastEndpoints.HealthChecks` | `/healthz` and `/readyz` are pinned by Docker health checks, Compose and tests. No gain. |
| AI agents (MCP/A2A), x402 payments | No requirement; would add a third transport around the same access model. |

## Page-by-page matrix

| Docs page | Status | Backlog items |
|---|---|---|
| Get Started | Standard (base types, `Configure()`, `Send.OkAsync`, cancellation). `Results<...>` union returns not adopted: they bypass the envelope builder. | — |
| Scaffolding | Layout matches the `feat` template; tooling optional. | — |
| Security | Standard (JWT via JWKS, secure by default, named policy). Optional `Scopes("cpnucleo.api")` adds nothing over the audience check. | — |
| Model binding | Standard except `required` on body DTOs. | 2 |
| Validation | Standard (`Validator<T>`, shared bases, camelCase keys). | 3, 14 |
| Dependency injection | Constructor injection everywhere except the notifier; generator registrations unused. | 5, 12 |
| Domain entity mapping | Equivalent (Mapperly + domain factories). | — |
| File handling | Not applicable. | — |
| Response caching | Not adopted (per-caller data). | — |
| Rate limiting | Standard (ASP.NET limiter with the envelope and `Retry-After`). | — |
| OpenAPI documents | Configured, but tags, names, responses and pagination are inaccurate. | 6–11 |
| Pre/post processors | Not used; middleware covers cross-cutting work. | 15, 19 |
| Exception handler | Equivalent custom middleware (required by the envelope rule). | — |
| API versioning | Not adopted (route paths). | — |
| Idempotency | Not adopted (per-caller data; creates already idempotent). | — |
| Event bus / command bus / command rules | Not used in-process. | 19 |
| Job queues | Lab only. | Labs |
| Remote procedure calls | Standard (contracts project, handler server, MessagePack). | 17 |
| Server-sent events | Native .NET 10 SSE; FastEndpoints sender gives shutdown cancellation and response tracking. | 13 |
| Integration & unit testing | `Factory.Create` standard; `FastEndpoints.Testing` unused. | 5, 18 |
| Configuration settings | Route prefix standard; error, warmup and discovery options unused. | 3, 9, 12 |
| Misc conveniences | Send helpers, `Options()` standard; custom senders miss `MarkResponseStart()`. | 4, 13 |
| Native AOT | `CreateSlimBuilder` standard; source generation unused; AOT stays an explicit experiment. | 12, Labs |
| Cookbook | Grouping, error responses, `IOperationProcessor`, spec export and unit-test service recipes apply; the rest are covered above. | 6, 9–11, 5 |

## Tests and pins to update when adopting

- `tests/Architecture.Tests/FastEndpointsConfigurationTests.cs:516`: exact string `UseFastEndpoints(c => c.Endpoints.RoutePrefix = "api")`. Any global configuration (#3, #7, #9, #12) needs a looser assertion that still forbids hard-coded `/api` routes.
- The same file pins `.SwaggerDocument(o =>`, `TagDescriptions`, `EnableJWTBearerAuth` and the security headers (relevant to #6–#11).
- `tests/WebApi.Integration.Tests` `ApiContractTests` and `PaginationContractTests`: envelope field keys (#2, #3).
- `src/WebClient/src/lib/api/http-client.test.ts:123`: mocked validation message (#3).
- `tests/WebApi.Unit.Tests/Common/EndpointTestExtensions.cs` (#5).
- `tests/WebApi.Integration.Tests/ConcurrencyAndStreamingTests.cs` (#13).

## Suggested pull requests

1. **Fix:** anonymous 500 (#1), with its integration test. Done in #271, together with this document.
2. **Contract hygiene:** #2, #3, #4 and #14, with the envelope tests.
3. **OpenAPI accuracy:** #6–#10, then the exported snapshot (#11).
4. **Endpoint plumbing:** #5, #13, #15 and #16, mostly mechanical.
5. **Startup and tooling:** #12 and #18, plus the gRPC handler items (#17).
