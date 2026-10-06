namespace WebApi.Common.Extensions;

/// <summary>
/// Error responses with the shared <see cref="ApiErrorResponse"/> body. FastEndpoints' built-in
/// <c>NotFoundAsync</c> starts an empty response, which bypasses the envelope.
/// </summary>
public static class ResponseSenderExtensions
{
    public static Task NotFoundEnvelopeAsync(this IResponseSender sender, CancellationToken cancellationToken) =>
        ApiErrors.WriteAsync(sender.HttpContext, StatusCodes.Status404NotFound, cancellationToken: cancellationToken);
}
