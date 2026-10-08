using System.Diagnostics;
using System.Globalization;
using System.Net;
using Dapper;
using FastEndpoints;
using Grpc.Core;
using GrpcStreamingLab;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Npgsql;
using Testcontainers.PostgreSql;

// A gRPC server stream (FastEndpoints Remote Messaging) fed by PostgreSQL LISTEN/NOTIFY, compared with
// the 15 s refresh loop that REST SSE listings use to see writes from other instances and transports.
CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
var fallback = TimeSpan.FromSeconds(Option("--fallback-seconds", 30));
var idle = TimeSpan.FromSeconds(Option("--idle-seconds", 16));
var sseRefresh = TimeSpan.FromSeconds(15);
var reconnectDelay = TimeSpan.FromSeconds(2);
var pushBudget = TimeSpan.FromSeconds(2);
var started = Stopwatch.GetTimestamp();

try
{
    await RunAsync();
    Console.WriteLine($"All checks passed in {Stopwatch.GetElapsedTime(started).TotalSeconds:0.0} s.");
    return 0;
}
catch (LabFailure failure)
{
    // A claimed property did not hold. Anything else (Docker unavailable, ...) surfaces as an unhandled exception.
    Console.Error.WriteLine($"FAILED: {failure.Message}");
    return 1;
}

async Task RunAsync()
{
    await using var database = new PostgreSqlBuilder("postgres:16.15").Build();
    await database.StartAsync();
    var connectionString = database.GetConnectionString();
    await using var admin = new NpgsqlConnection(connectionString);
    await admin.OpenAsync();
    await admin.ExecuteAsync($"""
        CREATE TABLE projects (
            id uuid PRIMARY KEY,
            name text NOT NULL,
            active boolean NOT NULL DEFAULT true,
            created_at timestamptz NOT NULL DEFAULT clock_timestamp(),
            updated_at timestamptz NOT NULL DEFAULT clock_timestamp(),
            deleted_at timestamptz);
        CREATE FUNCTION notify_projects_changed() RETURNS trigger LANGUAGE plpgsql AS $$
        BEGIN
            PERFORM pg_notify('{ProjectStore.Channel}', json_build_object('op', TG_OP, 'id', COALESCE(NEW.id, OLD.id))::text);
            RETURN NULL;
        END $$;
        CREATE TRIGGER projects_changed AFTER INSERT OR UPDATE OR DELETE ON projects
            FOR EACH ROW EXECUTE FUNCTION notify_projects_changed();
        INSERT INTO projects (id, name) SELECT gen_random_uuid(), 'Seed project ' || n FROM generate_series(1, 5) n;
        """);

    // Another API instance or the other transport: an independent connection the stream knows nothing about.
    await using var writer = new NpgsqlConnection(new NpgsqlConnectionStringBuilder(connectionString) { ApplicationName = "independent-writer" }.ConnectionString);
    await writer.OpenAsync();

    var store = new ProjectStore(connectionString);
    var probe = new LabProbe();
    var builder = WebApplication.CreateSlimBuilder();
    builder.Logging.SetMinimumLevel(LogLevel.Warning);
    builder.WebHost.ConfigureKestrel(kestrel => kestrel.Listen(IPAddress.Loopback, 0, listen => listen.Protocols = HttpProtocols.Http2));
    builder.AddHandlerServer();
    builder.Services.AddSingleton(store).AddSingleton(probe).AddSingleton(new StreamSettings(fallback, reconnectDelay));
    await using var app = builder.Build();
    app.MapHandlers(handlers => handlers.RegisterServerStream<WatchProjectsCommand, WatchProjectsHandler, ProjectsSnapshotDto>());
    await app.StartAsync();
    var address = app.Services.GetRequiredService<IServer>().Features.Get<IServerAddressesFeature>()!.Addresses.Single();
    app.MapRemote(address, remote => remote.RegisterServerStream<WatchProjectsCommand, ProjectsSnapshotDto>());
    Console.WriteLine($"Handler server on {address} (HTTP/2); PostgreSQL 16.15 in a disposable container.");
    Console.WriteLine($"Stream fallback refresh {fallback.TotalSeconds:0} s, re-subscribe backoff {reconnectDelay.TotalSeconds:0} s; compared polling loop {sseRefresh.TotalSeconds:0} s (the SSE design).");

    // The gRPC client: one server-stream call, every snapshot recorded with its arrival time.
    var stream = new SnapshotLog();
    using var clientCancel = new CancellationTokenSource();
    var streamOpened = Stopwatch.GetTimestamp();
    var consumer = Task.Run(async () =>
    {
        try
        {
            await foreach (var snapshot in new WatchProjectsCommand().RemoteExecuteAsync(clientCancel.Token))
                stream.Add(snapshot);
            return "completed";
        }
        catch (RpcException exception) when (exception.StatusCode == StatusCode.Cancelled)
        {
            return "RpcException(Cancelled)";
        }
        catch (OperationCanceledException)
        {
            return "OperationCanceledException";
        }
    });

    // The comparison: an SSE-style loop that sees external writes only through its periodic refresh.
    var polls = new SnapshotLog();
    using var pollingStop = new CancellationTokenSource();
    var polling = Task.Run(async () =>
    {
        try
        {
            polls.Add(await store.SnapshotAsync("poll", pollingStop.Token));
            while (true)
            {
                await Task.Delay(sseRefresh, pollingStop.Token);
                polls.Add(await store.SnapshotAsync("poll", pollingStop.Token));
            }
        }
        catch (OperationCanceledException) when (pollingStop.IsCancellationRequested)
        {
            // Expected: the comparison loop is stopped at the end of the run.
        }
    });

    // 1. Initial snapshot.
    var (initialLatency, initial) = await stream.WaitAsync(streamOpened, s => s.Reason == "initial", TimeSpan.FromSeconds(10), "Initial snapshot");
    Check(initial.Total == 5 && initial.Sequence == 1, $"initial snapshot had {initial.Total} projects, sequence {initial.Sequence}");
    Check(await ListenerBackendsAsync() == 1, "expected one LISTEN backend while the stream is open");
    Console.WriteLine($"[1] Initial snapshot: {initial.Total} projects, sequence {initial.Sequence}, {Ms(initialLatency)} after the call; 1 dedicated LISTEN backend (pid {probe.ListenerPid}).");

    // 2. Idle period: no writes at all.
    var streamQueriesBefore = StreamQueries();
    var pollQueriesBefore = store.Queries("poll");
    var snapshotsBefore = stream.Count;
    await Task.Delay(idle);
    var idleStreamQueries = StreamQueries() - streamQueriesBefore;
    var idlePollQueries = store.Queries("poll") - pollQueriesBefore;
    Check(stream.Count == snapshotsBefore, "the stream sent a snapshot while nothing changed");
    Check(idleStreamQueries <= (int)Math.Ceiling(idle / fallback), $"the stream ran {idleStreamQueries} queries while idle");
    Console.WriteLine($"[2] Idle {idle.TotalSeconds:0} s: stream ran {idleStreamQueries} refresh queries and sent nothing; the {sseRefresh.TotalSeconds:0} s loop ran {idlePollQueries}.");

    // 3. Push latency for single writes from the independent connection.
    var writes = new List<(string Kind, long Start, Func<ProjectsSnapshotDto, bool> Visible)>();
    var latencies = new List<double>();
    for (var round = 1; round <= 3; round++)
    {
        var id = Guid.CreateVersion7();
        var name = $"Pushed project {round}";
        var renamed = $"{name} (renamed)";
        foreach (var (kind, sql, visible) in new (string, string, Func<ProjectsSnapshotDto, bool>)[]
        {
            ("insert", "INSERT INTO projects (id, name) VALUES (@id, @name)", s => s.Items.Any(p => p.Id == id)),
            ("update", "UPDATE projects SET name = @renamed, updated_at = clock_timestamp() WHERE id = @id", s => s.Items.Any(p => p.Id == id && p.Name == renamed)),
            ("soft delete", "UPDATE projects SET active = false, deleted_at = clock_timestamp(), updated_at = clock_timestamp() WHERE id = @id", s => s.Items.All(p => p.Id != id))
        })
        {
            var start = Stopwatch.GetTimestamp();
            await writer.ExecuteAsync(sql, new { id, name, renamed });
            var (latency, seen) = await stream.WaitAsync(start, visible, TimeSpan.FromSeconds(10), $"{kind} of {name}");
            Check(seen.Reason == "notify", $"{kind} arrived through '{seen.Reason}', not a notification");
            Check(latency < pushBudget, $"{kind} took {Ms(latency)}, budget {Ms(pushBudget)}");
            writes.Add((kind, start, visible));
            latencies.Add(latency.TotalMilliseconds);
        }
    }
    latencies.Sort();
    Console.WriteLine($"[3] {latencies.Count} external writes (insert, rename, soft delete x3) pushed through NOTIFY: write-to-client min {latencies[0]:0.0} ms, median {Median(latencies):0.0} ms, max {latencies[^1]:0.0} ms (budget {Ms(pushBudget)}; SSE waits up to {sseRefresh.TotalSeconds:0} s).");

    // 4. A burst of rapid writes, each in its own transaction.
    const int Burst = 20;
    var notificationsBefore = probe.NotificationsReceived;
    var notifyQueriesBefore = store.Queries("notify");
    snapshotsBefore = stream.Count;
    var burstIds = Enumerable.Range(0, Burst).Select(_ => Guid.CreateVersion7()).ToList();
    var burstStart = Stopwatch.GetTimestamp();
    foreach (var id in burstIds)
        await writer.ExecuteAsync("INSERT INTO projects (id, name) VALUES (@id, 'Burst project')", new { id });
    var (burstLatency, _) = await stream.WaitAsync(burstStart, s => burstIds.All(id => s.Items.Any(p => p.Id == id)), TimeSpan.FromSeconds(10), "Burst");
    await WaitUntilAsync(() => Task.FromResult(probe.NotificationsReceived - notificationsBefore >= Burst), TimeSpan.FromSeconds(5), "burst notifications");
    await Task.Delay(300); // let the refresh for the last notification finish
    var burstNotifications = probe.NotificationsReceived - notificationsBefore;
    var burstQueries = store.Queries("notify") - notifyQueriesBefore;
    var burstSnapshots = stream.Since(snapshotsBefore);
    Check(burstNotifications == Burst, $"{burstNotifications} notifications for {Burst} writes");
    Check(burstSnapshots.Sum(s => s.NotificationsCoalesced) <= Burst, "snapshots absorbed more notifications than were sent");
    var outcome = burstQueries < Burst
        ? $"partly coalesced into {burstQueries} refresh queries (one absorbed up to {burstSnapshots.Max(s => s.NotificationsCoalesced)} notifications)"
        : "not coalesced: one refresh query per notification";
    outcome += $"; {burstSnapshots.Count} snapshots sent, {burstQueries - burstSnapshots.Count} refreshes unchanged and not sent";
    Console.WriteLine($"[4] Burst of {Burst} inserts: {burstNotifications} notifications, {outcome}; final state complete {Ms(burstLatency)} after the first write.");

    // 5. A write that never notifies (session_replication_role = replica skips ordinary triggers, as restores
    //    and logical replication do). Only the periodic fallback refresh can find it.
    notificationsBefore = probe.NotificationsReceived;
    var silentId = Guid.CreateVersion7();
    var silentStart = Stopwatch.GetTimestamp();
    await using (var transaction = await writer.BeginTransactionAsync())
    {
        await writer.ExecuteAsync("SET LOCAL session_replication_role = replica", transaction: transaction);
        await writer.ExecuteAsync("INSERT INTO projects (id, name) VALUES (@silentId, 'Silent project')", new { silentId }, transaction);
        await transaction.CommitAsync();
    }
    var (silentLatency, silentSeen) = await stream.WaitAsync(silentStart, s => s.Items.Any(p => p.Id == silentId), fallback + TimeSpan.FromSeconds(5), "Silent write");
    Check(probe.NotificationsReceived == notificationsBefore, "the trigger-less write produced a notification");
    Check(silentSeen.Reason == "fallback", $"silent write arrived through '{silentSeen.Reason}'");
    Console.WriteLine($"[5] Missed notification (trigger bypassed): no NOTIFY; the fallback refresh delivered it after {silentLatency.TotalSeconds:0.0} s (bound: {fallback.TotalSeconds:0} s since the previous refresh).");

    // 6. Kill the LISTEN backend, write while nobody listens, and converge after re-subscribing.
    var oldPid = probe.ListenerPid;
    var lost = probe.NextListenerLossAsync();
    var killStart = Stopwatch.GetTimestamp();
    Check(await admin.ExecuteScalarAsync<bool>("SELECT pg_terminate_backend(@oldPid)", new { oldPid }), "pg_terminate_backend failed");
    var lostAt = await lost.WaitAsync(TimeSpan.FromSeconds(5));
    var detection = Stopwatch.GetElapsedTime(killStart, lostAt);
    await WaitUntilAsync(async () => await ListenerBackendsAsync() == 0, TimeSpan.FromSeconds(1), "the killed backend to exit");
    var gapId = Guid.CreateVersion7();
    var gapStart = Stopwatch.GetTimestamp();
    await writer.ExecuteAsync("INSERT INTO projects (id, name) VALUES (@gapId, 'Written while nobody listened')", new { gapId });
    Check(await ListenerBackendsAsync() == 0, "the stream re-subscribed before the gap write; the scenario did not test a missed notification");
    var (gapLatency, gapSeen) = await stream.WaitAsync(gapStart, s => s.Items.Any(p => p.Id == gapId), reconnectDelay + TimeSpan.FromSeconds(5), "Write during the LISTEN gap");
    Check(gapSeen.Reason == "resubscribe", $"gap write arrived through '{gapSeen.Reason}'");
    Check(probe.ListenerPid != oldPid && await ListenerBackendsAsync() == 1, "no new LISTEN backend after re-subscribing");
    var afterId = Guid.CreateVersion7();
    var afterStart = Stopwatch.GetTimestamp();
    await writer.ExecuteAsync("INSERT INTO projects (id, name) VALUES (@afterId, 'After re-subscribe')", new { afterId });
    var (afterLatency, afterSeen) = await stream.WaitAsync(afterStart, s => s.Items.Any(p => p.Id == afterId), TimeSpan.FromSeconds(10), "Write after re-subscribe");
    Check(afterSeen.Reason == "notify" && afterLatency < pushBudget, $"post-recovery write took {Ms(afterLatency)} via '{afterSeen.Reason}'");
    Console.WriteLine($"[6] LISTEN backend {oldPid} terminated: loss detected in {Ms(detection)}; a write with no listener converged after {gapLatency.TotalSeconds:0.00} s via the re-subscribe refresh (backend {probe.ListenerPid}); the next write was pushed again in {Ms(afterLatency)}.");

    // 7. What the polling loop saw of the same single writes.
    var polled = writes.Select(w => polls.Find(w.Start, w.Visible) is { } hit ? Stopwatch.GetElapsedTime(w.Start, hit.Timestamp).TotalSeconds : (double?)null).ToList();
    var observed = polled.Where(seconds => seconds is not null).Select(seconds => seconds!.Value).Order().ToList();
    Console.WriteLine(observed.Count == 0
        ? $"[7] The {sseRefresh.TotalSeconds:0} s loop observed none of the {writes.Count} single-write states."
        : $"[7] The {sseRefresh.TotalSeconds:0} s loop observed {observed.Count} of {writes.Count} single-write states (the rest were overwritten before its next refresh), after {observed[0]:0.0}-{observed[^1]:0.0} s.");

    // 8. Client cancellation ends the server-side stream and releases the LISTEN backend.
    var ended = probe.StreamEndedAsync();
    var cancelStart = Stopwatch.GetTimestamp();
    await clientCancel.CancelAsync();
    var end = await ended.WaitAsync(TimeSpan.FromSeconds(5));
    var clientOutcome = await consumer.WaitAsync(TimeSpan.FromSeconds(5));
    Check(end.ObservedCancellation, "the handler ended without observing cancellation");
    await WaitUntilAsync(async () => await ListenerBackendsAsync() == 0, TimeSpan.FromSeconds(5), "the LISTEN backend to be released");
    var released = Stopwatch.GetElapsedTime(cancelStart);
    Console.WriteLine($"[8] Client cancelled ({clientOutcome}): handler observed cancellation after {Ms(Stopwatch.GetElapsedTime(cancelStart, end.Timestamp))}; LISTEN backend gone after {Ms(released)}.");

    await pollingStop.CancelAsync();
    await polling;
    var openFor = Stopwatch.GetElapsedTime(streamOpened, cancelStart);
    var writeCount = writes.Count + Burst + 3;
    Console.WriteLine($"[9] Queries while the stream was open ({openFor.TotalSeconds:0} s, {writeCount} external writes, {probe.NotificationsReceived} notifications received): stream {StreamQueries()} (initial {store.Queries("initial")}, notify {store.Queries("notify")}, fallback {store.Queries("fallback")}, resubscribe {store.Queries("resubscribe")}, degraded {store.Queries("degraded")}); {sseRefresh.TotalSeconds:0} s polling loop {store.Queries("poll")}.");
    Console.WriteLine($"    Per change: the stream runs at most one query per notification batch, per open stream; polling runs none extra but is up to {sseRefresh.TotalSeconds:0} s late and skips intermediate states.");
    Console.WriteLine($"    Idle, per open stream per hour: stream {3600 / fallback.TotalSeconds:0} fallback queries + 1 held connection; polling {3600 / sseRefresh.TotalSeconds:0} queries.");
    await app.StopAsync();

    int StreamQueries() => new[] { "initial", "notify", "fallback", "resubscribe", "degraded" }.Sum(store.Queries);

    async Task<int> ListenerBackendsAsync() => await admin.ExecuteScalarAsync<int>(
        "SELECT count(*) FROM pg_stat_activity WHERE application_name = @name", new { name = ProjectStore.ListenerApplicationName });
}

static void Check(bool condition, string failure)
{
    if (!condition) throw new LabFailure(failure);
}

static async Task WaitUntilAsync(Func<Task<bool>> condition, TimeSpan timeout, string what)
{
    var deadline = Stopwatch.GetTimestamp() + (long)(timeout.TotalSeconds * Stopwatch.Frequency);
    while (!await condition())
    {
        if (Stopwatch.GetTimestamp() > deadline) throw new LabFailure($"timed out waiting for {what}");
        await Task.Delay(10);
    }
}

static string Ms(TimeSpan value) => $"{value.TotalMilliseconds:0.0} ms";

static double Median(List<double> sorted) =>
    sorted.Count % 2 == 1 ? sorted[sorted.Count / 2] : (sorted[sorted.Count / 2 - 1] + sorted[sorted.Count / 2]) / 2;

int Option(string name, int fallbackValue)
{
    var index = Array.IndexOf(args, name);
    return index >= 0 && index + 1 < args.Length && int.TryParse(args[index + 1], out var value) && value > 0 ? value : fallbackValue;
}
