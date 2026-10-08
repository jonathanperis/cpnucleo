namespace IdentityApi.Common;

/// <summary>
/// Error responses with the shared <see cref="ApiErrorResponse"/> body. FastEndpoints' built-in
/// senders start an empty response, which bypasses the envelope.
/// </summary>
public static class ResponseSenderExtensions
{
    public static Task NotFoundEnvelopeAsync(this IResponseSender sender, string message, CancellationToken cancellationToken) =>
        sender.EnvelopeAsync(StatusCodes.Status404NotFound, message, cancellationToken);

    public static Task ForbiddenEnvelopeAsync(this IResponseSender sender, string message, CancellationToken cancellationToken) =>
        sender.EnvelopeAsync(StatusCodes.Status403Forbidden, message, cancellationToken);

    private static Task EnvelopeAsync(this IResponseSender sender, int statusCode, string message, CancellationToken cancellationToken)
    {
        // Custom senders must mark the response as started, as FastEndpoints' own senders do.
        sender.HttpContext.MarkResponseStart();
        return ApiErrors.WriteAsync(sender.HttpContext, statusCode, message, cancellationToken: cancellationToken);
    }
}
