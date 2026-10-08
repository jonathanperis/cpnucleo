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

    /// <summary>
    /// Streams the current request's live listing as server-sent <c>listing</c> events through
    /// FastEndpoints, which marks the response started, disables proxy buffering, numbers the events
    /// and ends the stream on application shutdown. Before every refresh the caller's session is
    /// validated again, so a deactivated account or revoked admin stops receiving data within one refresh.
    /// </summary>
    public static Task ListingStreamAsync<TResponse>(
        this IResponseSender sender,
        string resource,
        Func<CancellationToken, Task<TResponse>> getSnapshot,
        ListingChangeNotifier listingChanges,
        ILogger logger,
        CancellationToken cancellationToken) where TResponse : notnull
    {
        var context = sender.HttpContext;
        var sessions = context.RequestServices.GetRequiredService<TokenSessionValidator>();
        var stream = CreateListingStream(resource, getSnapshot, listingChanges, logger, cancellationToken,
            async token => await sessions.ValidateAsync(context.User, token) is null);
        return context.Response.SendEventStreamAsync("listing", stream, cancellationToken);
    }

    public static IAsyncEnumerable<TResponse> CreateListingStream<TResponse>(
        string resource,
        Func<CancellationToken, Task<TResponse>> getSnapshot,
        ListingChangeNotifier listingChanges,
        ILogger logger,
        CancellationToken cancellationToken,
        Func<CancellationToken, Task<bool>>? sessionIsValid = null) =>
        ReadListingSnapshots(resource, getSnapshot, listingChanges, logger, sessionIsValid, cancellationToken);

    private static async IAsyncEnumerable<TResponse> ReadListingSnapshots<TResponse>(
        string resource,
        Func<CancellationToken, Task<TResponse>> getSnapshot,
        ListingChangeNotifier listingChanges,
        ILogger logger,
        Func<CancellationToken, Task<bool>>? sessionIsValid,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var observedVersion = listingChanges.CurrentVersion(resource);
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

        yield return lastSnapshot;

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                observedVersion = await listingChanges.WaitForChangeAsync(resource, observedVersion, cancellationToken, TimeSpan.FromSeconds(15));
            }
            catch (TimeoutException)
            {
                // Other API instances and gRPC writers do not share this process's notifier.
                // A periodic snapshot also keeps idle proxy connections alive.
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                logger.LogDebug("Listing SSE stream disconnected by the client.");
                yield break;
            }

            TResponse nextSnapshot;
            try
            {
                if (sessionIsValid is not null && !await sessionIsValid(cancellationToken))
                {
                    logger.LogInformation("Listing SSE stream closed: the session is no longer valid.");
                    yield break;
                }

                nextSnapshot = await getSnapshot(cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                logger.LogDebug("Listing SSE stream disconnected by the client.");
                yield break;
            }

            yield return nextSnapshot;
        }
    }
}
