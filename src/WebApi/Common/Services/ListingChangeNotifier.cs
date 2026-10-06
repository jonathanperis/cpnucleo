namespace WebApi.Common.Services;

/// <summary>
/// Process-local change signal for live listings, tracked per resource so a write to one resource
/// only wakes the streams of that resource (and of resources it cascades to). Writes made by other
/// processes are picked up by the streams' periodic refresh.
/// </summary>
public sealed class ListingChangeNotifier
{
    private readonly object gate = new();
    private readonly Dictionary<string, Signal> signals = new(StringComparer.Ordinal);

    public long CurrentVersion(string resource)
    {
        lock (gate)
        {
            return GetSignal(resource).Version;
        }
    }

    public void NotifyChanged(params string[] resources)
    {
        foreach (var resource in resources.Distinct(StringComparer.Ordinal))
        {
            TaskCompletionSource<long> waiter;
            long nextVersion;
            lock (gate)
            {
                var signal = GetSignal(resource);
                nextVersion = ++signal.Version;
                waiter = signal.NextChange;
                signal.NextChange = CreateWaiter();
            }
            waiter.TrySetResult(nextVersion);
        }
    }

    public Task<long> WaitForChangeAsync(string resource, long observedVersion, CancellationToken cancellationToken, TimeSpan? timeout = null)
    {
        lock (gate)
        {
            var signal = GetSignal(resource);
            if (signal.Version != observedVersion) return Task.FromResult(signal.Version);
            return signal.NextChange.Task.WaitAsync(timeout ?? Timeout.InfiniteTimeSpan, cancellationToken);
        }
    }

    private Signal GetSignal(string resource)
    {
        if (!signals.TryGetValue(resource, out var signal))
        {
            signal = new Signal();
            signals[resource] = signal;
        }

        return signal;
    }

    private static TaskCompletionSource<long> CreateWaiter() =>
        new(TaskCreationOptions.RunContinuationsAsynchronously);

    private sealed class Signal
    {
        public long Version;
        public TaskCompletionSource<long> NextChange = CreateWaiter();
    }
}
