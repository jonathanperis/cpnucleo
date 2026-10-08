using System.Collections.Concurrent;
using System.Diagnostics;
using Dapper;
using FastEndpoints;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using Testcontainers.PostgreSql;

// FastEndpoints job queues (8.3.0) backed by a Dapper/PostgreSQL storage provider, compared with
// the hand-written transactional outbox in labs/OutboxLab. Everything runs in a disposable database;
// no real notification is sent.
var clock = Stopwatch.StartNew();
await using var database = new PostgreSqlBuilder("postgres:16.15").Build();
await database.StartAsync();
await using var dataSource = NpgsqlDataSource.Create(database.GetConnectionString());
await using (var schema = await dataSource.OpenConnectionAsync())
{
    await schema.ExecuteAsync(JobStorage.Schema + """
        CREATE TABLE appointments (id uuid PRIMARY KEY, description text NOT NULL);
        CREATE TABLE simulated_delivery_receipts (appointment_id uuid PRIMARY KEY, appointment_existed boolean NOT NULL);
        """);
}

// Job queues need a host for DI and the application lifetime, but no HTTP listener.
var builder = Host.CreateApplicationBuilder();
builder.Logging.ClearProviders(); // the lab reports its own observations; failures surface as exceptions
builder.Services.AddSingleton(dataSource);
builder.Services.AddSingleton<DeliveryProbe>();
builder.Services.AddJobQueues<JobRecord, JobStorage>();
using var host = builder.Build();
host.UseJobQueues(o =>
{
    o.MaxConcurrency = 4;
    o.ExecutionTimeLimit = TimeSpan.FromSeconds(10);
    o.StorageProbeDelay = TimeSpan.FromMilliseconds(200);
    o.RetryDelay = TimeSpan.FromMilliseconds(200);
    o.IdempotencyKeyFor<SendAppointmentReminder>(c => c.AppointmentId.ToString("D"));
});
await host.StartAsync();
var storage = host.Services.GetRequiredService<JobStorage>();
var probe = host.Services.GetRequiredService<DeliveryProbe>();

static void Require(bool condition, string failure)
{
    if (!condition) throw new InvalidOperationException(failure);
}

async Task<T> ScalarAsync<T>(string sql, object? parameters = null)
{
    await using var connection = await dataSource.OpenConnectionAsync();
    return await connection.ExecuteScalarAsync<T>(sql, parameters) ?? throw new InvalidOperationException($"No value: {sql}");
}

async Task WaitUntilAsync(string sql, object? parameters, string what)
{
    var deadline = Stopwatch.StartNew();
    while (!await ScalarAsync<bool>(sql, parameters))
    {
        if (deadline.Elapsed > TimeSpan.FromSeconds(30)) throw new TimeoutException($"Timed out waiting for {what}.");
        await Task.Delay(50);
    }
}

Task WaitForJobAsync(Guid trackingId) =>
    WaitUntilAsync("SELECT coalesce((SELECT iscomplete FROM jobs WHERE trackingid = @trackingId), false)", new { trackingId }, $"job {trackingId}");

async Task InsertAppointmentAsync(NpgsqlConnection connection, NpgsqlTransaction transaction, Guid id) =>
    await connection.ExecuteAsync("INSERT INTO appointments VALUES (@id, 'Review the learning exercise')", new { id }, transaction);

const string Facts = """
    SELECT (SELECT count(*) FROM appointments WHERE id = @id) AS appointments,
           (SELECT count(*) FROM jobs WHERE idempotencykey = @key) AS jobs,
           (SELECT appointment_existed FROM simulated_delivery_receipts WHERE appointment_id = @id) AS receipt
    """;

async Task<(long Appointments, long Jobs, bool? Receipt)> FactsAsync(Guid id)
{
    await using var connection = await dataSource.OpenConnectionAsync();
    return await connection.QuerySingleAsync<(long, long, bool?)>(Facts, new { id, key = id.ToString("D") });
}

// 1. The library path: QueueJobAsync inside a business transaction that rolls back.
var rolledBack = Guid.CreateVersion7();
Guid rolledBackJob;
await using (var connection = await dataSource.OpenConnectionAsync())
await using (var transaction = await connection.BeginTransactionAsync())
{
    await InsertAppointmentAsync(connection, transaction, rolledBack);
    rolledBackJob = await new SendAppointmentReminder { AppointmentId = rolledBack }.QueueJobAsync();
    await transaction.RollbackAsync();
}
await WaitForJobAsync(rolledBackJob);
var rollbackFacts = await FactsAsync(rolledBack);
Require(rollbackFacts is { Appointments: 0, Jobs: 1, Receipt: false },
    $"Expected the rolled-back appointment to leave its job behind and run; observed {rollbackFacts}.");
Console.WriteLine("Finding: QueueJobAsync is not a transactional outbox. The business transaction rolled back, yet its job stayed " +
                  "queued (stored on the provider's own connection) and ran a reminder for an appointment that does not exist.");

// 2. The library path again: QueueJobAsync before the business transaction commits.
var early = Guid.CreateVersion7();
await using (var connection = await dataSource.OpenConnectionAsync())
await using (var transaction = await connection.BeginTransactionAsync())
{
    await InsertAppointmentAsync(connection, transaction, early);
    await WaitForJobAsync(await new SendAppointmentReminder { AppointmentId = early }.QueueJobAsync());
    await transaction.CommitAsync();
}
var earlyFacts = await FactsAsync(early);
Require(earlyFacts is { Appointments: 1, Jobs: 1, Receipt: false },
    $"Expected the job to run before its appointment committed; observed {earlyFacts}.");
Console.WriteLine("Finding: a job queued before commit can run before the business write is visible; its handler saw no " +
                  "appointment although the appointment committed afterwards.");

// 3. Enlisting the job in the caller's transaction: CreateJob<JobRecord>() builds the record (queue id,
//    serialized command, idempotency key) and the lab's provider inserts it on the business connection.
var atomicRollback = Guid.CreateVersion7();
await using (var connection = await dataSource.OpenConnectionAsync())
await using (var transaction = await connection.BeginTransactionAsync())
{
    await InsertAppointmentAsync(connection, transaction, atomicRollback);
    await JobStorage.InsertAsync(new SendAppointmentReminder { AppointmentId = atomicRollback }.CreateJob<JobRecord>(), connection, transaction);
    await transaction.RollbackAsync();
}
var atomicRollbackFacts = await FactsAsync(atomicRollback);
Require(atomicRollbackFacts is { Appointments: 0, Jobs: 0, Receipt: null },
    $"The enlisted job did not roll back with its appointment; observed {atomicRollbackFacts}.");

// 4. The outbox-equivalent scenario: N reminders scheduled atomically and triggered after commit,
//    one injected crash after delivery, and a duplicate enqueue rejected by the idempotency key.
const int Reminders = 50;
var appointments = Enumerable.Range(0, Reminders).Select(_ => Guid.CreateVersion7()).ToArray();
probe.CrashOnceAfterDelivery(appointments[0]);
var jobs = new Dictionary<Guid, Guid>();
foreach (var id in appointments)
{
    var command = new SendAppointmentReminder { AppointmentId = id };
    var job = command.CreateJob<JobRecord>();
    await using (var connection = await dataSource.OpenConnectionAsync())
    await using (var transaction = await connection.BeginTransactionAsync())
    {
        await InsertAppointmentAsync(connection, transaction, id);
        await JobStorage.InsertAsync(job, connection, transaction);
        await transaction.CommitAsync();
    }
    command.TriggerJobExecution(); // wake the local queue now; distributed polling would find it anyway
    jobs[id] = job.TrackingID;
}

var duplicate = await new SendAppointmentReminder { AppointmentId = appointments[1] }.QueueJobAsync();
Require(duplicate == jobs[appointments[1]], "A duplicate enqueue did not return the existing tracking id.");

await WaitUntilAsync("SELECT NOT EXISTS (SELECT 1 FROM jobs WHERE NOT iscomplete)", null, "the queue to drain");
await using (var connection = await dataSource.OpenConnectionAsync())
{
    var keys = appointments.Select(id => id.ToString("D")).ToArray();
    var jobRows = await connection.ExecuteScalarAsync<int>("SELECT count(*) FROM jobs WHERE idempotencykey = ANY(@keys)", new { keys });
    var receipts = await connection.ExecuteScalarAsync<int>(
        "SELECT count(*) FROM simulated_delivery_receipts WHERE appointment_id = ANY(@appointments) AND appointment_existed", new { appointments });
    var crashed = await connection.QuerySingleAsync<(int Attempts, string? LastError, bool IsComplete)>(
        "SELECT attempts, lasterror, iscomplete FROM jobs WHERE trackingid = @id", new { id = jobs[appointments[0]] });
    var others = await connection.ExecuteScalarAsync<int>(
        "SELECT count(*) FROM jobs WHERE idempotencykey = ANY(@keys) AND attempts <> 0", new { keys });

    Require(jobRows == Reminders, $"Expected {Reminders} jobs (duplicate rejected), found {jobRows}.");
    Require(receipts == Reminders, $"Expected {Reminders} effects with a visible appointment, found {receipts}.");
    Require(crashed is { Attempts: 1, IsComplete: true } && crashed.LastError?.Contains(nameof(SimulatedCrash)) == true,
        $"The crashed job was not recorded as one failure followed by completion: {crashed}.");
    Require(others == 1, $"Expected exactly one job with a recorded failure, found {others}.");
}
Require(appointments.All(id => probe.Invocations(id) == (id == appointments[0] ? 2 : 1)),
    "Handler invocations were not one per job plus one retry for the crashed job.");
Require(probe.AbsorbedDuplicates == 1, $"Expected one duplicate delivery absorbed by the receipt key, saw {probe.AbsorbedDuplicates}.");
Require(appointments.All(id => probe.Effects(id) == 1), "A reminder was delivered with an effect count other than one.");

Console.WriteLine("Verified: CreateJob<JobRecord>() written through the business transaction is atomic; a rollback left neither appointment nor job.");
Console.WriteLine($"Verified: {Reminders} reminders scheduled atomically and triggered after commit each produced exactly one effect, " +
                  "and every handler saw its appointment.");
Console.WriteLine("Verified: idempotent queueing; re-queueing an existing appointment returned the original tracking id and stored no second job.");
Console.WriteLine("Verified: injected crash after delivery was recorded by OnHandlerExecutionFailureAsync, retried after the backoff, " +
                  "and the simulated provider's idempotency key absorbed the duplicate delivery.");

// 5. The claim query directly: concurrent claimers and an abandoned lease. These rows use their own
//    queue ids, so the running job queue never executes them.
const int ClaimRows = 40;
const int Claimers = 4;
await using (var connection = await dataSource.OpenConnectionAsync())
{
    foreach (var queue in Enumerable.Repeat("claim-lab", ClaimRows).Append("lease-lab"))
    {
        await JobStorage.InsertAsync(new JobRecord
        {
            TrackingID = Guid.CreateVersion7(),
            QueueID = queue,
            ExecuteAfter = DateTime.UtcNow.AddSeconds(-1),
            ExpireOn = DateTime.UtcNow.AddHours(1),
        }, connection, null);
    }
}
var claimsByWorker = new ConcurrentDictionary<int, List<Guid>>();
await Task.WhenAll(Enumerable.Range(1, Claimers).Select(async worker =>
{
    var mine = claimsByWorker.GetOrAdd(worker, _ => []);
    while (await storage.ClaimAsync("claim-lab", 3, TimeSpan.FromMinutes(1), CancellationToken.None) is { Count: > 0 } batch)
    {
        mine.AddRange(batch.Select(r => r.TrackingID));
        await Task.Yield();
    }
}));
var allClaims = claimsByWorker.Values.SelectMany(ids => ids).ToList();
Require(allClaims.Count == ClaimRows && allClaims.Distinct().Count() == ClaimRows,
    $"Concurrent claimers took {allClaims.Count} claims for {allClaims.Distinct().Count()} distinct of {ClaimRows} rows.");

var lease = TimeSpan.FromSeconds(1);
var firstClaim = await storage.ClaimAsync("lease-lab", 1, lease, CancellationToken.None);
var whileLeased = await storage.ClaimAsync("lease-lab", 1, lease, CancellationToken.None);
await Task.Delay(lease + TimeSpan.FromMilliseconds(300));
var afterExpiry = await storage.ClaimAsync("lease-lab", 1, lease, CancellationToken.None);
Require(firstClaim.Count == 1 && whileLeased.Count == 0 && afterExpiry.Count == 1 && afterExpiry[0].TrackingID == firstClaim[0].TrackingID,
    "An abandoned lease was not reclaimable exactly after it expired.");
foreach (var record in allClaims.Select(id => new JobRecord { TrackingID = id }).Append(afterExpiry[0]))
    await storage.MarkJobAsCompleteAsync(record, CancellationToken.None);
Require(await ScalarAsync<long>("SELECT count(*) FROM jobs WHERE NOT iscomplete") == 0, "Jobs remain pending.");

Console.WriteLine($"Verified: {Claimers} concurrent claimers took {ClaimRows} rows with no overlap " +
                  $"({string.Join(", ", claimsByWorker.OrderBy(p => p.Key).Select(p => $"#{p.Key}: {p.Value.Count}"))}).");
Console.WriteLine("Verified: a lease abandoned by a simulated crashed worker was invisible while held and claimable again after it expired.");
Console.WriteLine("Verified: nothing is left pending.");
Console.WriteLine("Delivery remains at-least-once; only the simulated provider's idempotency key prevents duplicate effects.");

await host.StopAsync();
Console.WriteLine(FormattableString.Invariant($"Completed in {clock.Elapsed.TotalSeconds:F1}s."));

sealed class SendAppointmentReminder : ICommand
{
    public Guid AppointmentId { get; set; }
}

sealed class SendAppointmentReminderHandler(NpgsqlDataSource dataSource, DeliveryProbe probe) : ICommandHandler<SendAppointmentReminder>
{
    public async Task ExecuteAsync(SendAppointmentReminder command, CancellationToken ct)
    {
        // A separate committed write simulates an external provider supporting an idempotency key.
        await using var connection = await dataSource.OpenConnectionAsync(ct);
        var inserted = await connection.ExecuteAsync(new CommandDefinition("""
            INSERT INTO simulated_delivery_receipts (appointment_id, appointment_existed)
            VALUES (@id, EXISTS (SELECT 1 FROM appointments WHERE id = @id))
            ON CONFLICT DO NOTHING
            """, new { id = command.AppointmentId }, cancellationToken: ct));
        probe.Record(command.AppointmentId, inserted == 1);
        if (probe.ShouldCrash(command.AppointmentId)) throw new SimulatedCrash();
    }
}

sealed class DeliveryProbe
{
    readonly ConcurrentDictionary<Guid, int> invocations = new();
    readonly ConcurrentDictionary<Guid, int> effects = new();
    Guid crashTarget;
    int crashed;
    int absorbed;

    public int AbsorbedDuplicates => Volatile.Read(ref absorbed);
    public int Invocations(Guid id) => invocations.GetValueOrDefault(id);
    public int Effects(Guid id) => effects.GetValueOrDefault(id);
    public void CrashOnceAfterDelivery(Guid id) => crashTarget = id;
    public bool ShouldCrash(Guid id) => id == crashTarget && Interlocked.Exchange(ref crashed, 1) == 0;

    public void Record(Guid id, bool newEffect)
    {
        invocations.AddOrUpdate(id, 1, (_, count) => count + 1);
        if (newEffect) effects.AddOrUpdate(id, 1, (_, count) => count + 1);
        else Interlocked.Increment(ref absorbed);
    }
}

sealed class SimulatedCrash() : Exception("Injected crash after delivery, before the job was marked complete.");
