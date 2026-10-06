using Microsoft.AspNetCore.Http;

namespace Infrastructure.Http;

/// <summary>
/// Translates exceptions into the <see cref="ApiErrorResponse"/> envelope without leaking internal
/// messages: only <see cref="DomainException"/> and <see cref="AccessDeniedException"/> messages
/// (written for clients) are returned verbatim.
/// </summary>
public sealed class ApiExceptionMiddleware(RequestDelegate next, ILogger<ApiExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
        {
            logger.LogDebug("Request {Method} {Path} was cancelled by the client.", context.Request.Method, context.Request.Path);
            if (!context.Response.HasStarted) context.Response.StatusCode = 499;
        }
        catch (Exception exception) when (!context.Response.HasStarted && Translate(exception) is { } error)
        {
            logger.LogInformation("Request rejected with {StatusCode}: {Reason}", error.StatusCode, error.LogReason);
            context.Response.Clear();
            await ApiErrors.WriteAsync(context, error.StatusCode, error.Message, error.Errors, context.RequestAborted);
        }
// Intentional catch-all at the middleware boundary: unexpected failures are logged with details
// server-side and returned as a generic 500 envelope.
#pragma warning disable CA1031
        catch (Exception exception) when (!context.Response.HasStarted)
#pragma warning restore CA1031
        {
            logger.LogError(exception, "An unhandled exception occurred while processing {Method} {Path}.", context.Request.Method, context.Request.Path);
            context.Response.Clear();
            await ApiErrors.WriteAsync(context, StatusCodes.Status500InternalServerError, cancellationToken: context.RequestAborted);
        }
    }

    private sealed record TranslatedError(int StatusCode, string Message, IReadOnlyDictionary<string, string[]>? Errors, string LogReason);

    private static TranslatedError? Translate(Exception exception)
    {
        if (exception is DomainException domain)
            return new(StatusCodes.Status400BadRequest, domain.Message, ApiErrors.FieldError(domain.Field, domain.Message), "domain rule");
        if (exception is AccessDeniedException denied)
            return new(StatusCodes.Status403Forbidden, denied.Message, null, "access denied");
        if (exception is RecordNotFoundException missing)
            return new(StatusCodes.Status404NotFound, missing.Message, null, "not found or not visible");
        if (exception is ArgumentException)
            return new(StatusCodes.Status400BadRequest, "The request contains an invalid value.", null, "invalid argument");

        return DatabaseErrors.Classify(exception) switch
        {
            { Kind: DatabaseErrorKind.Duplicate } e => new(StatusCodes.Status409Conflict, e.Message, null, "duplicate"),
            { Kind: DatabaseErrorKind.InactiveReference } e => new(StatusCodes.Status400BadRequest, e.Message,
                ApiErrors.FieldError(e.Field, e.Message), "inactive reference"),
            { Kind: DatabaseErrorKind.ActiveDependents } e => new(StatusCodes.Status409Conflict, e.Message, null, "active dependents"),
            { Kind: DatabaseErrorKind.InvalidValue } e => new(StatusCodes.Status400BadRequest, e.Message, null, "invalid value"),
            { Kind: DatabaseErrorKind.ConcurrentChange } e => new(StatusCodes.Status409Conflict, e.Message, null, "concurrent change"),
            _ => null
        };
    }
}
