namespace WebApi.Common.Services;

public sealed class ListingChangeNotifier
{
    private readonly object gate = new();
    private long version;
    private TaskCompletionSource<long> nextChange = CreateWaiter();

    public long CurrentVersion
    {
        get
        {
            lock (gate)
            {
                return version;
            }
        }
    }

    public void NotifyChanged()
    {
        TaskCompletionSource<long> waiter;
        long nextVersion;

        lock (gate)
        {
            version++;
            nextVersion = version;
            waiter = nextChange;
            nextChange = CreateWaiter();
        }

        waiter.TrySetResult(nextVersion);
    }

    public Task<long> WaitForChangeAsync(long observedVersion, CancellationToken cancellationToken)
    {
        lock (gate)
        {
            if (version != observedVersion) return Task.FromResult(version);
            return nextChange.Task.WaitAsync(cancellationToken);
        }
    }

    private static TaskCompletionSource<long> CreateWaiter() =>
        new(TaskCreationOptions.RunContinuationsAsynchronously);
}
