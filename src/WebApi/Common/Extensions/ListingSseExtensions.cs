namespace WebApi.Common.Extensions;

public static class ListingSseExtensions
{
    private static readonly TimeSpan RefreshInterval = TimeSpan.FromSeconds(10);

    public static bool AcceptsServerSentEvents(this HttpRequest request)
    {
        foreach (var value in request.Headers.Accept.Where(value => !string.IsNullOrWhiteSpace(value)).Select(value => value!))
        {
            foreach (var part in value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                if (!MediaTypeWithQualityHeaderValue.TryParse(part, out var mediaType)) continue;
                if (!"text/event-stream".Equals(mediaType.MediaType, StringComparison.OrdinalIgnoreCase)) continue;
                if (mediaType.Quality is null or > 0) return true;
            }
        }

        return false;
    }

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
        TResponse lastSnapshot;
        try
        {
            lastSnapshot = await getSnapshot(cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            logger.LogDebug("Listing SSE stream disconnected by the client.");
            yield break;
        }

        var lastSnapshotJson = JsonSerializer.Serialize(lastSnapshot);
        yield return lastSnapshot;

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(RefreshInterval, cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                logger.LogDebug("Listing SSE stream disconnected by the client.");
                yield break;
            }

            TResponse nextSnapshot;
            try
            {
                nextSnapshot = await getSnapshot(cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                logger.LogDebug("Listing SSE stream disconnected by the client.");
                yield break;
            }

            var nextSnapshotJson = JsonSerializer.Serialize(nextSnapshot);
            if (nextSnapshotJson == lastSnapshotJson) continue;

            lastSnapshot = nextSnapshot;
            lastSnapshotJson = nextSnapshotJson;
            yield return lastSnapshot;
        }
    }
}
