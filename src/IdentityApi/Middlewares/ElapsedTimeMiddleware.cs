namespace IdentityApi.Middlewares;

public class ElapsedTimeMiddleware(RequestDelegate next, ILogger<ElapsedTimeMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        logger.LogInformation("Request {Method} {Path} starting.", context.Request.Method, context.Request.Path);

        var stopwatch = Stopwatch.StartNew();

        await next(context);

        stopwatch.Stop();

        var elapsedTimeMs = stopwatch.Elapsed.TotalMilliseconds;

        logger.LogInformation("Request {Method} {Path} executed in {ElapsedTime} ms.", context.Request.Method, context.Request.Path, elapsedTimeMs);

        // Save the elapsed time in HttpContext.Items for access by subsequent middleware.
        context.Items["ElapsedTime"] = elapsedTimeMs;
    }
}