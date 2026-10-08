# Learning Lab

Cpnucleo is a practical school for application engineering. Start with a working baseline, change one variable, observe the behavior, and explain the tradeoff before introducing another tool.

## Learning paths

| Level | Path | Observable result |
|---|---|---|
| Beginner | Start the tiny lab; sign in; complete one CRUD cycle; inspect a request and its DTO | A record travels from native form to endpoint to PostgreSQL and back |
| Intermediate | Compare EF Core, explicit Dapper and generic UoW; run transport parity and rollback tests | Different implementations preserve the same contract |
| Intermediate | Modify an assignment date/hour rule; exercise it through both transports | Invalid state is rejected by domain behavior |
| Advanced | Run stale-write, SSE, performance and restore exercises | Failure boundaries and operational costs become measurable |

## Maturity labels

- **Working baseline:** authenticated CRUD, Astro/native TypeScript, hashing, bounded paging, soft deletion, schema migrations and immutable deployment.
- **Verified examples:** transactional project batches, version-aware project writes, duplicate-login contention, HTTP/gRPC parity and cross-instance SSE convergence.
- **Incremental pilot:** Application-layer sharing currently begins with project creation. Other transports still show direct orchestration deliberately.
- **Exercises:** tenant isolation, durable background jobs, outbox delivery and Native AOT are not advertised as implemented guarantees.
- **Isolated experiments (`labs/`):** the outbox, FastEndpoints job queues, gRPC streaming with LISTEN/NOTIFY, OpenAPI generators, Scalar and Native AOT run in disposable environments; their findings are observations, not features of the deployed hosts.

## 1. Follow one request

Trace project creation from `CrudPage.astro` through `crud-controller.ts`, `webapi-client.ts`, the REST endpoint, `Application/Features/Projects/CreateProject`, `ProjectCreateStore`, and PostgreSQL. Then read it through gRPC.

```sh
dotnet test tests/WebApi.Integration.Tests/ --filter FullyQualifiedName~RestCreate_GrpcReadAndRemove
```

Expected: both transports agree on identity; removal makes the project unavailable while preserving the row and parent relationship. Compare this with an EF-backed resource in `CrudContractTests`.

## 2. Observe atomicity and concurrency

```sh
dotnet test tests/WebApi.Integration.Tests/ --filter FullyQualifiedName~MixedValidityBatch
dotnet test tests/WebApi.Integration.Tests/ --filter FullyQualifiedName~StaleProjectEdit
```

Expected: a batch containing a missing ID changes nothing. Two clients editing the same project with one observed version produce one successful write and one conflict.

Temporarily remove the transaction or expected-version predicate on a learning branch. Observe the regression, then restore it. Existing clients that omit `expectedVersion` intentionally retain last-write-wins compatibility; the native UI supplies it for project edits.

## 3. Distinguish authentication from authorization

Run `Security.Unit.Tests`, `AuthenticationTests` and `AuthorizationTests`. A valid identity proves who is calling; `ResourceAccess` decides what they may touch. Members see and change only their projects' data, catalog writes need an admin claim, and the same answers come back over REST and gRPC whether EF Core or Dapper serves the request. Tokens stop working within 30 seconds of a password change, deactivation or admin removal. Refresh preserves an eight-hour original-session boundary.

Exercise: move the membership rule for assignments from `ResourceAccess` into one endpoint only, run `AuthorizationTests`, and watch the other transport fail. Restore the shared rule.

Tenant claims are informational today. A genuine tenant-isolation exercise must carry tenant identity through EF filters, Dapper predicates, foreign-key ownership and authorization tests. Merely adding a claim is not isolation.

## 4. Study replicated real-time delivery

```sh
dotnet test tests/WebApi.Integration.Tests/ --filter FullyQualifiedName~Sse_Refreshes
```

Expected: an external database write reaches the stream within the next 15-second refresh cycle. Same-process writes notify immediately. This is bounded convergence, not durable event delivery. Section 9 compares it with a PostgreSQL LISTEN/NOTIFY experiment, including missed notifications and reconnects; neither is durable delivery.

## 5. Measure persistence

```sh
dotnet run --project labs/PersistenceLab -c Release -- --rows 2000 --iterations 50
```

The experiment creates a disposable PostgreSQL container, warms both approaches, and measures equivalent ordered 25-row-plus-count reads using EF Core and Dapper. Change dataset size and iteration count. Record runtime, architecture and timings together. These are local educational measurements, not universal performance claims.

## 6. Recover data and inspect traces

With the local lab running:

```sh
bash scripts/lab-restore.sh
docker compose -f compose.lab.yaml --profile observability up -d
```

The restore exercise creates a fresh temporary database inside the lab container, restores a logical dump, compares record counts, and removes only that temporary database. The source lab remains intact.

Open Grafana at localhost:3000. Perform an API request and correlate the HTTP trace, SQL activity, logs and service-instance labels. Compare Debug and optimized Release. `EventSourceSupport` and HTTP activity propagation stay enabled in the release configuration so the experiment remains observable.

## 7. Durable appointment reminders: isolated outbox experiment

```sh
dotnet run --project labs/OutboxLab -c Release
```

The experiment saves an appointment and its outgoing event in one transaction, lets competing workers claim rows, injects a crash after simulated delivery, and retries with an idempotency key. It verifies one delivery effect and no pending event. Everything runs in a disposable database; no real notification is sent.

Delivery is at-least-once, not magically exactly-once. The simulated provider commits independently of the worker and deduplicates its effect. Extend the experiment with retry schedules, poison messages and an actual provider only after defining those contracts. This lab is not a deployed reminder service.

## 8. FastEndpoints job queues: isolated comparison with the outbox

```sh
dotnet run --project labs/JobQueueLab -c Release
```

The experiment hosts FastEndpoints job queues (8.3.0, no HTTP listener) on a Dapper/PostgreSQL storage provider in distributed mode. Jobs are claimed with a lease (`DequeueAfter`) by one atomic `UPDATE … FOR UPDATE SKIP LOCKED`, and a unique `(QueueID, IdempotencyKey)` index enforces idempotent queueing.

It first records a finding: `QueueJobAsync` is **not** a transactional outbox. The provider stores the job on its own connection, so a rolled-back appointment still leaves its job queued and the reminder runs for an appointment that does not exist; a job queued before commit can also run before the appointment is visible. It then shows the transactional path the library allows: `CreateJob<JobRecord>()` written through the business transaction plus `TriggerJobExecution()` after commit, where a rollback leaves neither appointment nor job. With that path, 50 reminders each produce exactly one simulated effect, re-queueing returns the original tracking id, an injected crash after delivery is retried and absorbed by the simulated provider's idempotency key, four concurrent claimers take 40 rows with no overlap, and an abandoned lease becomes claimable again after it expires.

What this does not show: only one job-queue host runs; competing claims call the provider's claim query directly rather than running separate processes. Transactional enqueueing is this lab's use of `CreateJob`, not a FastEndpoints guarantee. Delivery stays at-least-once, with no retry cap or dead-letter queue (failures retry every second until `ExpireOn`, 4 hours by default). The hourly stale-job purge is implemented but never reached in a short run. No real notification is sent.

## 9. Live lists over gRPC: server streaming with LISTEN/NOTIFY

```sh
dotnet run --project labs/GrpcStreamingLab -c Release
```

The gRPC counterpart of the REST SSE listings. A FastEndpoints Remote Messaging server-stream handler (`WatchProjectsCommand : IServerStreamCommand<ProjectsSnapshotDto>`) and its client run in one process over loopback HTTP/2. A trigger on a disposable `projects` table calls `pg_notify`; the handler `LISTEN`s on a dedicated connection, sends an initial snapshot, then a fresh snapshot after each batch of notifications, with a 30-second fallback refresh. A 15-second polling loop (how SSE sees other instances' writes) runs alongside for comparison.

It verifies, and exits non-zero otherwise: the initial snapshot; inserts, renames and soft deletes from an independent connection arriving within a 2-second budget; a 20-write burst ending in a complete snapshot; a write that bypasses the trigger arriving through the fallback refresh; detection of a terminated LISTEN backend followed by convergence after re-subscribing; and cancellation ending the handler and releasing its LISTEN backend. Observed locally: external write to client in about 4–15 ms (worst 58 ms) versus up to 15 s for SSE; loss of the listener detected in about 10–20 ms; the polling loop saw only 3 of 9 single-write states. Idle cost per open stream is about 120 fallback queries an hour plus one held connection, against 240 queries an hour for 15-second polling, but each change costs one query per open stream.

What this does not show: it is not wired into `GrpcServer` or `WebApi`, and has no authentication, session revalidation, access filtering or token-expiry close. One LISTEN connection per stream does not scale (a real host would share one listener and fan out) and does not work through transaction-pooling PgBouncer. NOTIFY is not durable: it is lost while nobody listens and is never sent for writes that bypass triggers. Detection was shown only for a terminated backend; silent network drops need keepalives. Timings are local measurements.

## 10. OpenAPI generators: FastEndpoints.OpenApi beside NSwag

```sh
dotnet run --project labs/OpenApiLab -c Release
```

The experiment hosts exactly WebApi's endpoints (`WebApi.DiscoveredTypes.All`) in-process with FastEndpoints.OpenApi 8.3.0 (Microsoft.AspNetCore.OpenApi) instead of FastEndpoints.Swagger (NSwag), and compares three generated documents with the reviewed contract in `docs/openapi/webapi.v1.json`: out of the box, with WebApi's equivalent settings, and with `PaginationQueryProcessor` and `ErrorResponsesProcessor` ported as operation transformers. It asserts only that both generators describe the same path + verb operations, and reports everything else.

Observed: out of the box, generation fails with HTTP 500, because `[DefaultValue("<uuid>")]` strings on `Guid` properties make Microsoft.AspNetCore.OpenApi throw; the lab works around it in schema metadata only. With equivalent settings, operations, operation ids, tags, security and required request properties match, but only FastEndpoints' default responses are listed, list endpoints advertise unbounded keys and filters the API rejects, and schema names collide (`Request2`, …). The ported transformers close the response, envelope and query-key gaps. Remaining differences: `uuid` instead of `guid` formats, `minItems` instead of `minLength` on arrays, no `additionalProperties: false`, flattened inherited DTOs, no component schemas for GET request DTOs, 33 missing property descriptions and OpenAPI 3.0.4 instead of 3.0.0.

What this does not show: that a migration is cheap. Switching generators would rewrite the reviewed contract with a large diff, needs typed `[DefaultValue]` examples (or the workaround), a port of the processors to Microsoft.OpenApi types, and the same work for IdentityApi. It does not render a UI or measure generation cost. The out-of-the-box attempt runs in its own process, because a second FastEndpoints host in the same process changed the result. This is an experiment, not a recommendation to replace NSwag.

## 11. API reference UI: Scalar beside Swagger UI

```sh
dotnet run --project labs/ApiReferenceLab -c Release
dotnet run --project labs/ApiReferenceLab -c Release -- --serve   # keep the host up for a browser
```

The lab serves Scalar.AspNetCore against the committed contracts (`docs/openapi/*.json`, read from the repository) with WebApi's security headers, in two profiles: Scalar defaults under WebApi's `/swagger` CSP, and a hardened profile with default fonts, telemetry, the AI agent, MCP and the hosted-client link disabled and a per-request script nonce. It fails unless both documents are served byte-identical and every script, favicon and document the reference pages use is same-origin, returns 200 and is allowed by the response CSP.

Observed: all scripts ship inside the package; with defaults only fonts come from a CDN (14 files from `fonts.scalar.com`, blocked by WebApi's `/swagger` CSP, so system fonts are used). With the hardened options, a local Chrome run rendered both references making only same-origin requests under `default-src 'none'; script-src 'nonce-…'; style-src 'unsafe-inline'; img-src 'self'; connect-src 'self'; frame-ancestors 'none'; base-uri 'none'; form-action 'none'`. Styles still need `'unsafe-inline'` (Scalar adds style elements and attributes without the nonce); `'unsafe-eval'`, `worker-src` and `font-src` were not needed.

What this does not show: the lab itself never renders the page; rendering and the absence of cross-origin requests come from one local browser run, not CI. The request client was not used against a live API (that needs `connect-src` to the API origin and a token). Scalar is not wired into WebApi or IdentityApi; Swagger UI remains the served UI.

## 12. Native AOT: compatibility experiment for WebApi

```sh
dotnet run --project labs/NativeAotLab -c Release
```

The experiment publishes the real WebApi with the opt-in `-p:AOT=true` switch for the current machine, groups the trim (IL2xxx) and AOT (IL3xxx) diagnostics by code and package family, and starts each binary against a disposable PostgreSQL container, with a JIT publish of the same commit as the baseline. It probes `/healthz`, `/readyz`, the OpenAPI document, an anonymous request (401 envelope), validation, and authenticated Dapper and EF Core reads and writes, and tries the native `--migrate-database` path. A second native variant applies FastEndpoints' remaining AOT guidance (generated reflection cache, reflection-based JSON standing in for serializer contexts) to a scratch copy of `src/` only. The report and logs go to the lab's `bin/.../reports/` folder; the exit code is 0 when the experiment ran, whatever it observed. It takes several minutes and is not run in CI.

Observed on 2026-10-08 (macOS arm64, SDK 10.0.401, current `main` with source-generated endpoint discovery):

- FastEndpoints.Swagger 8.3.0's build targets set `SuppressTrimAnalysisWarnings` and `SuppressAotAnalysisWarnings` in the consuming project, so a plain AOT publish reports almost nothing. With the suppression lifted, the publish reports 1,050 unique diagnostics: EF Core 700, System.Linq.Dynamic.Core 176, the Npgsql EF Core provider 59, Dapper 46, and a few dozen in BCL, Bogus, OpenTelemetry, Delta and our own code.
- The native binary (58 MB) publishes and gets past endpoint discovery, then exits at startup in Delta's static constructor, which reads `Assembly.Location` (empty in a native app; the IL3000 warning predicted it). The guided variant fails at the same place.
- The native migrator fails immediately: EF Core requires a compiled model under Native AOT.
- The JIT baseline passed every probe (about 1.4 s to healthy, about 205 MB resident).

What this does not show: that Native AOT is achievable or worthwhile for Cpnucleo. It is one machine and one run, with no Linux image, and IdentityApi and GrpcServer are not covered. A failing probe shows only the first break, not the full list of work (an EF Core compiled model or precompiled queries, serializer contexts, a Delta replacement, Dapper.AOT, System.Linq.Dynamic.Core). Few warnings do not mean compatible: unannotated reflection fails at run time without a warning. Native AOT stays an exercise, not a supported build.

## Decisions and contribution rhythm

See `docs/adr/0001-learning-baseline.md`. Keep meaningful technology comparisons, share domain rules, and label experiments honestly. Run focused tests first, then the required architecture and integration checks. Merge through a PR after CI passes.
