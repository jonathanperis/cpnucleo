namespace IdentityApi.Common;

/// <summary>
/// Error responses with the shared <see cref="ApiErrorResponse"/> body. FastEndpoints' built-in
/// senders start an empty response, which bypasses the envelope.
/// </summary>
public static class ResponseSenderExtensions
{
    public static Task NotFoundEnvelopeAsync(this IResponseSender sender, string message, CancellationToken cancellationToken) =>
        ApiErrors.WriteAsync(sender.HttpContext, StatusCodes.Status404NotFound, message, cancellationToken: cancellationToken);
}
