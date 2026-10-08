using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Channels;
using Dapper;
using FastEndpoints;
using Npgsql;

namespace GrpcStreamingLab;

public sealed record StreamSettings(TimeSpan FallbackInterval, TimeSpan ReconnectDelay);

/// <summary>The listing query. Every execution is counted by reason so the lab can compare query cost.</summary>
public sealed class ProjectStore(string connectionString)
{
    public const string Channel = "projects_changed";
    public const string ListenerApplicationName = "grpc-lab-listener";
    private const int PageSize = 50;

    private readonly ConcurrentDictionary<string, int> _queries = new();

    /// <summary>
    /// The LISTEN connection is dedicated and unpooled: a pooled connection would stay open after the
    /// stream ends, so the lab could not show that the backend is released.
    /// </summary>
    public string ListenerConnectionString { get; } = new NpgsqlConnectionStringBuilder(connectionString)
    {
        Pooling = false,
        ApplicationName = ListenerApplicationName
    }.ConnectionString;

    public int Queries(string reason) => _queries.GetValueOrDefault(reason);

    public async Task<ProjectsSnapshotDto> SnapshotAsync(string reason, CancellationToken ct)
    {
        _queries.AddOrUpdate(reason, 1, (_, count) => count + 1);
        await using var connection = new NpgsqlConnection(connectionString);
        var rows = (await connection.QueryAsync<ProjectRow>(new CommandDefinition("""
            SELECT id AS Id, name AS Name, updated_at AS UpdatedAt, count(*) OVER () AS Total
            FROM projects
            WHERE active
            ORDER BY updated_at DESC, id
            LIMIT @PageSize
            """, new { PageSize }, cancellationToken: ct))).AsList();
        return new ProjectsSnapshotDto
        {
            Reason = reason,
            Total = rows.Count == 0 ? 0 : (int)rows[0].Total,
            Items = rows.Select(row => new ProjectDto { Id = row.Id, Name = row.Name, UpdatedAt = row.UpdatedAt }).ToList(),
            ServerTimeUtc = DateTime.UtcNow
        };
    }

    private sealed record ProjectRow(Guid Id, string Name, DateTime UpdatedAt, long Total);
}

/// <summary>Server-side observations the lab asserts on; the handler never reads them.</summary>
public sealed class LabProbe
{
    private TaskCompletionSource<long> _listenerLost = NewSource<long>();
    private TaskCompletionSource<StreamEnd> _streamEnded = NewSource<StreamEnd>();
    private int _notifications;
    private int _listenerPid;

    public int NotificationsReceived => Volatile.Read(ref _notifications);
    public int ListenerPid => Volatile.Read(ref _listenerPid);
    public Task<long> NextListenerLossAsync() => Volatile.Read(ref _listenerLost).Task;
    public Task<StreamEnd> StreamEndedAsync() => _streamEnded.Task;

    public void NotificationReceived() => Interlocked.Increment(ref _notifications);
    public void ListenerConnected(int pid) => Volatile.Write(ref _listenerPid, pid);

    public void ListenerLost() =>
        Interlocked.Exchange(ref _listenerLost, NewSource<long>()).TrySetResult(Stopwatch.GetTimestamp());

    public void StreamEnded(bool observedCancellation) =>
        _streamEnded.TrySetResult(new StreamEnd(observedCancellation, Stopwatch.GetTimestamp()));

    private static TaskCompletionSource<T> NewSource<T>() => new(TaskCreationOptions.RunContinuationsAsynchronously);
}

public sealed record StreamEnd(bool ObservedCancellation, long Timestamp);

public readonly record struct ListenerSignal(int Notifications, bool Lost, bool Cancelled);

/// <summary>
/// One LISTEN connection. A pump task keeps <see cref="NpgsqlConnection.WaitAsync(CancellationToken)"/>
/// running so notifications are read as they arrive; a broken connection is reported as a signal.
/// </summary>
public sealed class ProjectsListener(string connectionString, LabProbe probe) : IAsyncDisposable
{
    private readonly Channel<bool> _signals = System.Threading.Channels.Channel.CreateUnbounded<bool>(); // true = NOTIFY, false = lost
    private NpgsqlConnection? _connection;
    private CancellationTokenSource? _pumpStop;
    private Task? _pump;
    private volatile bool _listening;

    public bool IsListening => _listening;

    public async Task StartAsync(CancellationToken ct)
    {
        var connection = new NpgsqlConnection(connectionString);
        connection.Notification += (_, _) =>
        {
            probe.NotificationReceived();
            _signals.Writer.TryWrite(true);
        };
        try
        {
            await connection.OpenAsync(ct);
            await using var listen = new NpgsqlCommand($"LISTEN {ProjectStore.Channel}", connection);
            await listen.ExecuteNonQueryAsync(ct);
        }
        catch
        {
            await connection.DisposeAsync();
            throw;
        }

        _connection = connection;
        _listening = true;
        probe.ListenerConnected(connection.ProcessID);
        _pumpStop = new CancellationTokenSource();
        _pump = PumpAsync(connection, _pumpStop.Token);
    }

    private async Task PumpAsync(NpgsqlConnection connection, CancellationToken stop)
    {
        try
        {
            while (true) await connection.WaitAsync(stop);
        }
        catch (OperationCanceledException) when (stop.IsCancellationRequested)
        {
        }
        catch (Exception)
        {
            // pg_terminate_backend, a failover or a network drop: NOTIFY messages sent from now until a new
            // LISTEN commits are lost for this stream. The handler must refresh after re-subscribing.
            _listening = false;
            probe.ListenerLost();
            _signals.Writer.TryWrite(false);
        }
    }

    /// <summary>Waits for notifications, a lost listener, the timeout or cancellation; drains everything queued.</summary>
    public async Task<ListenerSignal> WaitAsync(TimeSpan timeout, CancellationToken ct)
    {
        using var deadline = CancellationTokenSource.CreateLinkedTokenSource(ct);
        deadline.CancelAfter(timeout > TimeSpan.Zero ? timeout : TimeSpan.Zero);
        try
        {
            await _signals.Reader.WaitToReadAsync(deadline.Token);
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            return new ListenerSignal(0, false, true);
        }
        catch (OperationCanceledException)
        {
            // Fallback interval elapsed.
        }

        int notifications = 0;
        var lost = false;
        while (_signals.Reader.TryRead(out var signal))
        {
            if (signal) notifications++;
            else lost = true;
        }
        return new ListenerSignal(notifications, lost, false);
    }

    public async Task<bool> TryRestartAsync(CancellationToken ct)
    {
        await StopAsync();
        try
        {
            await StartAsync(ct);
            return true;
        }
        catch (Exception exception) when (exception is NpgsqlException or OperationCanceledException)
        {
            return false;
        }
    }

    private async Task StopAsync()
    {
        _listening = false;
        if (_pumpStop is not null) await _pumpStop.CancelAsync();
        if (_pump is not null) await _pump;
        if (_connection is not null) await _connection.DisposeAsync();
        _pumpStop?.Dispose();
        (_connection, _pumpStop, _pump) = (null, null, null);
    }

    public async ValueTask DisposeAsync() => await StopAsync();
}

/// <summary>
/// The gRPC counterpart of the REST SSE listing (ListingSseExtensions): an initial snapshot, then a fresh
/// snapshot when PostgreSQL reports a change, with a periodic fallback refresh for notifications that never
/// arrive. Unlike the SSE loop it only sends a snapshot when the listing actually changed.
/// </summary>
public sealed class WatchProjectsHandler(ProjectStore store, LabProbe probe, StreamSettings settings)
    : IServerStreamCommandHandler<WatchProjectsCommand, ProjectsSnapshotDto>
{
    public async IAsyncEnumerable<ProjectsSnapshotDto> ExecuteAsync(
        WatchProjectsCommand command, [EnumeratorCancellation] CancellationToken ct)
    {
        var listener = new ProjectsListener(store.ListenerConnectionString, probe);
        try
        {
            // LISTEN before the first query: a change committed between the two is then either in the
            // snapshot or queued as a notification, never in neither.
            if (!await TryAsync(() => listener.StartAsync(ct), ct)) yield break;
            var snapshot = await TrySnapshotAsync("initial", 0, ct);
            if (snapshot is null) yield break;

            long sequence = 0;
            snapshot.Sequence = ++sequence;
            var fingerprint = Fingerprint(snapshot);
            yield return snapshot;

            var fallbackDue = Stopwatch.GetTimestamp() + Ticks(settings.FallbackInterval);
            while (true)
            {
                var timeout = listener.IsListening
                    ? Stopwatch.GetElapsedTime(Stopwatch.GetTimestamp(), fallbackDue)
                    : settings.ReconnectDelay;
                var signal = await listener.WaitAsync(timeout, ct);
                if (signal.Cancelled) yield break;
                if (signal.Lost) continue; // back off for ReconnectDelay before re-subscribing

                string reason;
                if (!listener.IsListening)
                    reason = await listener.TryRestartAsync(ct) ? "resubscribe" : "degraded";
                else
                    reason = signal.Notifications > 0 ? "notify" : "fallback";

                var next = await TrySnapshotAsync(reason, signal.Notifications, ct);
                if (next is null) yield break;
                fallbackDue = Stopwatch.GetTimestamp() + Ticks(settings.FallbackInterval);

                var nextFingerprint = Fingerprint(next);
                if (nextFingerprint == fingerprint) continue;
                fingerprint = nextFingerprint;
                next.Sequence = ++sequence;
                yield return next;
            }
        }
        finally
        {
            await listener.DisposeAsync();
            probe.StreamEnded(ct.IsCancellationRequested);
        }
    }

    private async Task<ProjectsSnapshotDto?> TrySnapshotAsync(string reason, int notifications, CancellationToken ct)
    {
        try
        {
            var snapshot = await store.SnapshotAsync(reason, ct);
            snapshot.NotificationsCoalesced = notifications;
            return snapshot;
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            return null;
        }
    }

    private static async Task<bool> TryAsync(Func<Task> action, CancellationToken ct)
    {
        try
        {
            await action();
            return true;
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            return false;
        }
    }

    private static long Ticks(TimeSpan interval) => (long)(interval.TotalSeconds * Stopwatch.Frequency);

    private static int Fingerprint(ProjectsSnapshotDto snapshot)
    {
        var hash = new HashCode();
        hash.Add(snapshot.Total);
        foreach (var item in snapshot.Items)
        {
            hash.Add(item.Id);
            hash.Add(item.Name);
            hash.Add(item.UpdatedAt);
        }
        return hash.ToHashCode();
    }
}
