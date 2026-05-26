namespace WebApi.Common.Extensions;

public static class ListingSseExtensions
{
    public static bool AcceptsServerSentEvents(this HttpRequest request) =>
        request.Headers.Accept
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .Select(value => value!)
            .SelectMany(value => value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            .Select(ParseMediaType)
            .Where(mediaType => mediaType is not null)
            .Select(mediaType => mediaType!)
            .Any(mediaType =>
                "text/event-stream".Equals(mediaType.MediaType, StringComparison.OrdinalIgnoreCase) &&
                mediaType.Quality is null or > 0);

    private static MediaTypeWithQualityHeaderValue? ParseMediaType(string part) =>
        MediaTypeWithQualityHeaderValue.TryParse(part, out var mediaType) ? mediaType : null;

    public static IAsyncEnumerable<TResponse> CreateListingStream<TResponse>(
        Func<CancellationToken, Task<TResponse>> getSnapshot,
        ListingChangeNotifier listingChanges,
        ILogger logger,
        CancellationToken cancellationToken) =>
        ReadListingSnapshots(getSnapshot, listingChanges, logger, cancellationToken);

    private static async IAsyncEnumerable<TResponse> ReadListingSnapshots<TResponse>(
        Func<CancellationToken, Task<TResponse>> getSnapshot,
        ListingChangeNotifier listingChanges,
        ILogger logger,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var observedVersion = listingChanges.CurrentVersion;
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
                observedVersion = await listingChanges.WaitForChangeAsync(observedVersion, cancellationToken);
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
