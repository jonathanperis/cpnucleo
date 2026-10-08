namespace WebApi.Common.Extensions;

/// <summary>
/// Error responses with the shared <see cref="ApiErrorResponse"/> body. FastEndpoints' built-in
/// <c>NotFoundAsync</c> starts an empty response, which bypasses the envelope.
/// </summary>
public static class ResponseSenderExtensions
{
    public static Task NotFoundEnvelopeAsync(this IResponseSender sender, CancellationToken cancellationToken) =>
        sender.EnvelopeAsync(StatusCodes.Status404NotFound, null, cancellationToken);

    public static Task ConflictEnvelopeAsync(this IResponseSender sender, string message, CancellationToken cancellationToken) =>
        sender.EnvelopeAsync(StatusCodes.Status409Conflict, message, cancellationToken);

    /// <summary>A 429 with <c>Retry-After</c> in whole seconds (at least one); <paramref name="message"/> receives them.</summary>
    public static Task TooManyRequestsEnvelopeAsync(this IResponseSender sender, TimeSpan retryAfter, Func<int, string> message, CancellationToken cancellationToken)
    {
        var seconds = Math.Max(1, (int)Math.Ceiling(retryAfter.TotalSeconds));
        sender.HttpContext.Response.Headers.RetryAfter = seconds.ToString(System.Globalization.CultureInfo.InvariantCulture);
        return sender.EnvelopeAsync(StatusCodes.Status429TooManyRequests, message(seconds), cancellationToken);
    }

    private static Task EnvelopeAsync(this IResponseSender sender, int statusCode, string? message, CancellationToken cancellationToken)
    {
        // Custom senders must mark the response as started, as FastEndpoints' own senders do.
        sender.HttpContext.MarkResponseStart();
        return ApiErrors.WriteAsync(sender.HttpContext, statusCode, message, cancellationToken: cancellationToken);
    }
}
