using System.Diagnostics;

namespace GrpcStreamingLab;

/// <summary>Snapshots in arrival order with the local arrival time, so waits can measure latency.</summary>
public sealed class SnapshotLog
{
    private readonly Lock _gate = new();
    private readonly List<(long Timestamp, ProjectsSnapshotDto Snapshot)> _entries = [];
    private TaskCompletionSource _arrived = new(TaskCreationOptions.RunContinuationsAsynchronously);

    public int Count
    {
        get { lock (_gate) return _entries.Count; }
    }

    public IReadOnlyList<ProjectsSnapshotDto> Since(int index)
    {
        lock (_gate) return _entries.Skip(index).Select(entry => entry.Snapshot).ToList();
    }

    public void Add(ProjectsSnapshotDto snapshot)
    {
        TaskCompletionSource arrived;
        lock (_gate)
        {
            _entries.Add((Stopwatch.GetTimestamp(), snapshot));
            arrived = _arrived;
            _arrived = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        }
        arrived.TrySetResult();
    }

    /// <summary>The first snapshot that arrived at or after <paramref name="after"/> and satisfies the predicate.</summary>
    public (long Timestamp, ProjectsSnapshotDto Snapshot)? Find(long after, Func<ProjectsSnapshotDto, bool> predicate)
    {
        lock (_gate)
            return _entries.Where(entry => entry.Timestamp >= after && predicate(entry.Snapshot))
                .Select(entry => ((long Timestamp, ProjectsSnapshotDto Snapshot)?)entry)
                .FirstOrDefault();
    }

    public async Task<(TimeSpan Latency, ProjectsSnapshotDto Snapshot)> WaitAsync(
        long after, Func<ProjectsSnapshotDto, bool> predicate, TimeSpan timeout, string what)
    {
        using var deadline = new CancellationTokenSource(timeout);
        while (true)
        {
            Task arrived;
            lock (_gate) arrived = _arrived.Task;
            if (Find(after, predicate) is { } match)
                return (Stopwatch.GetElapsedTime(after, match.Timestamp), match.Snapshot);
            try
            {
                await arrived.WaitAsync(deadline.Token);
            }
            catch (OperationCanceledException)
            {
                throw new LabFailure($"{what}: no matching snapshot within {timeout.TotalSeconds:0.#} s.");
            }
        }
    }
}

public sealed class LabFailure(string message) : Exception(message);
