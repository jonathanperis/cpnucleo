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
await ScheduleAsync(commit: true);

async Task DeliverAsync(bool crashAfterDelivery)
{
    await using var worker = new NpgsqlConnection(database.GetConnectionString());
    await worker.OpenAsync();
    await using var transaction = await worker.BeginTransactionAsync();
    var id = await worker.QuerySingleOrDefaultAsync<Guid?>(
        "SELECT id FROM outbox WHERE NOT processed ORDER BY id LIMIT 1 FOR UPDATE SKIP LOCKED", transaction: transaction);
    if (id is null) return;

    // A separate committed transaction simulates an external provider supporting
    // an idempotency key. No real notification or external HTTP call is sent.
    await using var provider = new NpgsqlConnection(database.GetConnectionString());
    await provider.ExecuteAsync("INSERT INTO simulated_delivery_receipts VALUES (@id) ON CONFLICT DO NOTHING", new { id });
    if (crashAfterDelivery) throw new SimulatedCrash();

    await worker.ExecuteAsync("UPDATE outbox SET processed = true WHERE id = @id", new { id }, transaction);
    await transaction.CommitAsync();
}

try { await DeliverAsync(crashAfterDelivery: true); }
catch (SimulatedCrash) { Console.WriteLine("Injected crash after delivery, before acknowledging the outbox row."); }
await Task.WhenAll(DeliverAsync(false), DeliverAsync(false));
var deliveries = await connection.ExecuteScalarAsync<int>("SELECT count(*) FROM simulated_delivery_receipts");
var pending = await connection.ExecuteScalarAsync<int>("SELECT count(*) FROM outbox WHERE NOT processed");
if (deliveries != 1 || pending != 0) throw new InvalidOperationException("Delivery/retry contract failed.");
Console.WriteLine("Verified: atomic scheduling, competing workers, crash recovery and idempotent simulated delivery.");
Console.WriteLine("Delivery remains at-least-once; the simulated provider's idempotency key prevents duplicate effects.");

sealed class SimulatedCrash : Exception;
