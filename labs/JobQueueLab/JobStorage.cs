using System.Text.Json;
using Dapper;
using FastEndpoints;
using Npgsql;

// A FastEndpoints job storage record persisted by Dapper. Unquoted PostgreSQL column names
// fold to lower case, and Dapper maps them back to these properties case-insensitively.
sealed class JobRecord : IJobStorageRecord, IHasIdempotencyKey
{
    public Guid TrackingID { get; set; }
    public string QueueID { get; set; } = "";
    public object Command { get; set; } = null!; // not mapped: CommandJson is the persisted form
    public string CommandJson { get; set; } = "{}";
    public DateTime ExecuteAfter { get; set; }
    public DateTime ExpireOn { get; set; }
    public DateTime DequeueAfter { get; set; } = DateTime.UnixEpoch; // lease end; UTC so Npgsql accepts it
    public bool IsComplete { get; set; }
    public string? IdempotencyKey { get; set; }
    public int Attempts { get; set; }

    public TCommand GetCommand<TCommand>() where TCommand : class, ICommandBase
        => JsonSerializer.Deserialize<TCommand>(CommandJson)
           ?? throw new InvalidOperationException($"Job {TrackingID} has no command payload.");

    public void SetCommand<TCommand>(TCommand command) where TCommand : class, ICommandBase
    {
        Command = command;
        CommandJson = JsonSerializer.Serialize(command);
    }
}

// Dapper + PostgreSQL storage provider in distributed mode: jobs are claimed with a lease
// (DequeueAfter) by an atomic UPDATE over FOR UPDATE SKIP LOCKED rows.
sealed class JobStorage(NpgsqlDataSource dataSource) : IJobStorageProvider<JobRecord>
{
    public const string Schema = """
        CREATE TABLE jobs (
            trackingid uuid PRIMARY KEY,
            queueid text NOT NULL,
            commandjson jsonb NOT NULL,
            executeafter timestamptz NOT NULL,
            expireon timestamptz NOT NULL,
            dequeueafter timestamptz NOT NULL,
            iscomplete boolean NOT NULL DEFAULT false,
            idempotencykey text,
            attempts integer NOT NULL DEFAULT 0,
            lasterror text);
        CREATE UNIQUE INDEX jobs_idempotency ON jobs (queueid, idempotencykey) WHERE idempotencykey IS NOT NULL;
        CREATE INDEX jobs_pending ON jobs (queueid, iscomplete, executeafter, expireon, dequeueafter);
        """;

    // The lease outlives the handler's execution time limit by this margin.
    public static readonly TimeSpan LeaseMargin = TimeSpan.FromSeconds(5);
    // A failed job becomes eligible again after this delay (no attempt cap: it retries until ExpireOn).
    public static readonly TimeSpan RetryBackoff = TimeSpan.FromSeconds(1);

    public bool DistributedJobProcessingEnabled => true;

    // QueueJobAsync path: the provider opens its own connection, so the insert commits on its
    // own, independently of any business transaction the caller may have open.
    public async Task StoreJobAsync(JobRecord r, CancellationToken ct)
    {
        await using var connection = await dataSource.OpenConnectionAsync(ct);
        await InsertAsync(r, connection, null, ct);
    }

    // Shared insert. Passing the caller's connection/transaction enlists the job in that
    // transaction (used with CreateJob<JobRecord>() + TriggerJobExecution()).
    public static async Task InsertAsync(JobRecord r, NpgsqlConnection connection, NpgsqlTransaction? transaction, CancellationToken ct = default)
    {
        var inserted = await connection.ExecuteScalarAsync<Guid?>(new CommandDefinition("""
            INSERT INTO jobs (trackingid, queueid, commandjson, executeafter, expireon, dequeueafter, idempotencykey)
            VALUES (@TrackingID, @QueueID, @CommandJson::jsonb, @ExecuteAfter, @ExpireOn, @DequeueAfter, @IdempotencyKey)
            ON CONFLICT (queueid, idempotencykey) WHERE idempotencykey IS NOT NULL DO NOTHING
            RETURNING trackingid
            """, new { r.TrackingID, r.QueueID, r.CommandJson, r.ExecuteAfter, r.ExpireOn, r.DequeueAfter, r.IdempotencyKey },
            transaction, cancellationToken: ct));
        if (inserted is not null) return;

        var existing = await connection.ExecuteScalarAsync<Guid>(new CommandDefinition(
            "SELECT trackingid FROM jobs WHERE queueid = @QueueID AND idempotencykey = @IdempotencyKey",
            new { r.QueueID, r.IdempotencyKey }, transaction, cancellationToken: ct));
        throw new DuplicateJobException(existing, r.IdempotencyKey, r.QueueID);
    }

    // In distributed mode the library's Match predicate is: QueueID matches, not complete,
    // ExecuteAfter <= now, ExpireOn >= now and DequeueAfter <= now. ClaimAsync encodes it in SQL.
    public async Task<ICollection<JobRecord>> GetNextBatchAsync(PendingJobSearchParams<JobRecord> p)
    {
        var lease = p.ExecutionTimeLimit == Timeout.InfiniteTimeSpan ? TimeSpan.FromMinutes(30) : p.ExecutionTimeLimit + LeaseMargin;
        return await ClaimAsync(p.QueueID, p.Limit, lease, p.CancellationToken);
    }

    public async Task<List<JobRecord>> ClaimAsync(string queueId, int limit, TimeSpan lease, CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        await using var connection = await dataSource.OpenConnectionAsync(ct);
        var claimed = await connection.QueryAsync<JobRecord>(new CommandDefinition("""
            UPDATE jobs SET dequeueafter = @leaseUntil
            WHERE trackingid IN (
                SELECT trackingid FROM jobs
                WHERE queueid = @queueId AND NOT iscomplete
                  AND executeafter <= @now AND expireon >= @now AND dequeueafter <= @now
                ORDER BY executeafter
                LIMIT @limit
                FOR UPDATE SKIP LOCKED)
            RETURNING trackingid, queueid, commandjson::text AS commandjson, executeafter, expireon,
                      dequeueafter, iscomplete, idempotencykey, attempts
            """, new { queueId, limit, now, leaseUntil = now + lease }, cancellationToken: ct));
        return claimed.AsList();
    }

    public async Task MarkJobAsCompleteAsync(JobRecord r, CancellationToken ct)
    {
        await using var connection = await dataSource.OpenConnectionAsync(ct);
        await connection.ExecuteAsync(new CommandDefinition(
            "UPDATE jobs SET iscomplete = true WHERE trackingid = @TrackingID", new { r.TrackingID }, cancellationToken: ct));
    }

    public async Task CancelJobAsync(Guid trackingId, CancellationToken ct)
    {
        await using var connection = await dataSource.OpenConnectionAsync(ct);
        await connection.ExecuteAsync(new CommandDefinition(
            "UPDATE jobs SET iscomplete = true WHERE trackingid = @trackingId", new { trackingId }, cancellationToken: ct));
    }

    // Releases the lease and reschedules the job; the library fetches it again with a later batch.
    public async Task OnHandlerExecutionFailureAsync(JobRecord r, Exception exception, CancellationToken ct)
    {
        var retryAt = DateTime.UtcNow + RetryBackoff;
        var error = $"{exception.GetType().Name}: {exception.Message}";
        await using var connection = await dataSource.OpenConnectionAsync(ct);
        await connection.ExecuteAsync(new CommandDefinition("""
            UPDATE jobs SET attempts = attempts + 1, lasterror = left(@error, 500), executeafter = @retryAt, dequeueafter = @retryAt
            WHERE trackingid = @TrackingID
            """, new { r.TrackingID, error, retryAt }, cancellationToken: ct));
    }

    // Called hourly by the library, so a short lab run never reaches it.
    public async Task PurgeStaleJobsAsync(StaleJobSearchParams<JobRecord> p)
    {
        await using var connection = await dataSource.OpenConnectionAsync(p.CancellationToken);
        await connection.ExecuteAsync(new CommandDefinition(
            "DELETE FROM jobs WHERE iscomplete OR expireon <= @now", new { now = DateTime.UtcNow }, cancellationToken: p.CancellationToken));
    }
}
