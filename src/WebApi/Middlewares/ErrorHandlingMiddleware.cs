namespace WebApi.Middlewares;

public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ArgumentException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new { message = ex.Message }, context.RequestAborted);
        }
        catch (Exception ex) when (ex is Npgsql.PostgresException { SqlState: "23505" } ||
                                  ex.InnerException is Npgsql.PostgresException { SqlState: "23505" })
        {
            context.Response.StatusCode = StatusCodes.Status409Conflict;
            await context.Response.WriteAsJsonAsync(new { message = "A record with this identity already exists." }, context.RequestAborted);
        }
        catch (Exception ex) when (ex is Npgsql.PostgresException { SqlState: "23503" or "23514" } ||
                                  ex.InnerException is Npgsql.PostgresException { SqlState: "23503" or "23514" })
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new { message = "The relationship or value is invalid." }, context.RequestAborted);
        }
// Intentional catch-all at the middleware boundary to prevent unhandled exceptions
// from leaking to the client. All exceptions are logged server-side.
#pragma warning disable CA1031
        catch (Exception ex)
#pragma warning restore CA1031
        {
            logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Set the response details.
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        
        // Create a generic error response.
        var response = new { error = "An unexpected error occurred." };
        return context.Response.WriteAsJsonAsync(response);
    }
}
