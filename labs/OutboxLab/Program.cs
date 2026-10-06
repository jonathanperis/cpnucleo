using System.Collections.Concurrent;
using Dapper;
using Npgsql;
using Testcontainers.PostgreSql;

await using var database = new PostgreSqlBuilder("postgres:16.15").Build();
await database.StartAsync();
await using var connection = new NpgsqlConnection(database.GetConnectionString());
await connection.OpenAsync();
await connection.ExecuteAsync("""
    CREATE TABLE appointments (id uuid PRIMARY KEY, description text NOT NULL);
    CREATE TABLE outbox (id uuid PRIMARY KEY, processed boolean NOT NULL DEFAULT false);
    CREATE TABLE simulated_delivery_receipts (id uuid PRIMARY KEY);
    """);

async Task ScheduleAsync(bool commit)
{
    await using var transaction = await connection.BeginTransactionAsync();
    await connection.ExecuteAsync("""
        INSERT INTO appointments VALUES (@id, 'Review the learning exercise');
        INSERT INTO outbox (id) VALUES (@id);
        """, new { id = Guid.CreateVersion7() }, transaction);
    if (commit) await transaction.CommitAsync();
    else await transaction.RollbackAsync();
}

await ScheduleAsync(commit: false);
if (await connection.ExecuteScalarAsync<int>("SELECT (SELECT count(*) FROM appointments) + (SELECT count(*) FROM outbox)") != 0)
    throw new InvalidOperationException("The failed schedule was not atomic.");
const int Messages = 50;
const int Workers = 4;
for (var i = 0; i < Messages; i++) await ScheduleAsync(commit: true);

var acknowledgements = new ConcurrentDictionary<Guid, int>();
var deliveriesByWorker = new ConcurrentDictionary<int, int>();

async Task<bool> DeliverAsync(bool crashAfterDelivery, int workerId = 0)
{
    await using var worker = new NpgsqlConnection(database.GetConnectionString());
    await worker.OpenAsync();
    await using var transaction = await worker.BeginTransactionAsync();
    var id = await worker.QuerySingleOrDefaultAsync<Guid?>(
        "SELECT id FROM outbox WHERE NOT processed ORDER BY id LIMIT 1 FOR UPDATE SKIP LOCKED", transaction: transaction);
    if (id is null) return false;

    // A separate committed transaction simulates an external provider supporting
    // an idempotency key. No real notification or external HTTP call is sent.
    await using var provider = new NpgsqlConnection(database.GetConnectionString());
    await provider.ExecuteAsync("INSERT INTO simulated_delivery_receipts VALUES (@id) ON CONFLICT DO NOTHING", new { id });
    if (crashAfterDelivery) throw new SimulatedCrash();

    await worker.ExecuteAsync("UPDATE outbox SET processed = true WHERE id = @id", new { id }, transaction);
    await transaction.CommitAsync();
    acknowledgements.AddOrUpdate(id.Value, 1, (_, count) => count + 1);
    deliveriesByWorker.AddOrUpdate(workerId, 1, (_, count) => count + 1);
    return true;
}

try { await DeliverAsync(crashAfterDelivery: true); }
catch (SimulatedCrash) { Console.WriteLine("Injected crash after delivery, before acknowledging the outbox row."); }

// Competing workers drain the outbox. FOR UPDATE SKIP LOCKED hands each row to one worker at a
// time; the crashed row is simply claimed again because its acknowledgement never committed.
await Task.WhenAll(Enumerable.Range(1, Workers).Select(async workerId =>
{
    while (await DeliverAsync(crashAfterDelivery: false, workerId)) { }
}));
var deliveries = await connection.ExecuteScalarAsync<int>("SELECT count(*) FROM simulated_delivery_receipts");
var pending = await connection.ExecuteScalarAsync<int>("SELECT count(*) FROM outbox WHERE NOT processed");
if (deliveries != Messages || pending != 0 || acknowledgements.Count != Messages || acknowledgements.Values.Any(count => count != 1))
    throw new InvalidOperationException("Delivery/retry contract failed.");
Console.WriteLine($"Workers acknowledged {string.Join(", ", deliveriesByWorker.OrderBy(pair => pair.Key).Select(pair => $"#{pair.Key}: {pair.Value}"))} of {Messages} messages.");
Console.WriteLine("Verified: atomic scheduling, competing workers each acknowledging a message exactly once, crash recovery and idempotent simulated delivery.");
Console.WriteLine("Delivery remains at-least-once; the simulated provider's idempotency key prevents duplicate effects.");

sealed class SimulatedCrash : Exception;
