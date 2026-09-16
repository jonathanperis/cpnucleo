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

Run `Security.Unit.Tests` and the `UserAdministration` integration scenario. A valid identity is sufficient for shared-workspace operations, but user administration needs an admin claim. Refresh checks an active account, recalculates admin privileges and preserves an eight-hour original-session boundary.

Tenant claims are informational today. A genuine tenant-isolation exercise must carry tenant identity through EF filters, Dapper predicates, foreign-key ownership and authorization tests. Merely adding a claim is not isolation.

## 4. Study replicated real-time delivery

```sh
dotnet test tests/WebApi.Integration.Tests/ --filter FullyQualifiedName~Sse_Refreshes
```

Expected: an external database write reaches the stream within the next 15-second refresh cycle. Same-process writes notify immediately. This is bounded convergence, not durable event delivery. Compare its query cost with a future PostgreSQL LISTEN/NOTIFY experiment; account for missed notifications and reconnects before claiming stronger guarantees.

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

## Decisions and contribution rhythm

See `docs/adr/0001-learning-baseline.md`. Keep meaningful technology comparisons, share domain rules, and label experiments honestly. Run focused tests first, then the required architecture and integration checks. Merge through a PR after CI passes.
