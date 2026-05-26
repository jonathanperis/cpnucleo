namespace WebApi.Common.Extensions;

public static class ListingSseExtensions
{
    private static readonly TimeSpan RefreshInterval = TimeSpan.FromSeconds(10);

    public static bool AcceptsServerSentEvents(this HttpRequest request) =>
        request.Headers.Accept.Any(value => value?.Contains("text/event-stream", StringComparison.OrdinalIgnoreCase) == true);

    public static IAsyncEnumerable<TResponse> CreateListingStream<TResponse>(
        Func<CancellationToken, Task<TResponse>> getSnapshot,
        ILogger logger,
        CancellationToken cancellationToken) =>
        ReadListingSnapshots(getSnapshot, logger, cancellationToken);

    private static async IAsyncEnumerable<TResponse> ReadListingSnapshots<TResponse>(
        Func<CancellationToken, Task<TResponse>> getSnapshot,
        ILogger logger,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            yield return await getSnapshot(cancellationToken);

            try
            {
                await Task.Delay(RefreshInterval, cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                logger.LogDebug("Listing SSE stream disconnected by the client.");
                yield break;
            }
        }
    }
}
