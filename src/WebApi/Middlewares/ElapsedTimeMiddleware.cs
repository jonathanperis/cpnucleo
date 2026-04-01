namespace WebApi.Middlewares;

public class ElapsedTimeMiddleware(RequestDelegate next, ILogger<ElapsedTimeMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var method = context.Request.Method.Replace("\r", string.Empty).Replace("\n", string.Empty);
        var path = context.Request.Path.ToString().Replace("\r", string.Empty).Replace("\n", string.Empty);

        logger.LogInformation("Request {Method} {Path} starting.", method, path);

        var stopwatch = Stopwatch.StartNew();

        await next(context);

        stopwatch.Stop();

        var elapsedTimeMs = stopwatch.Elapsed.TotalMilliseconds;

        logger.LogInformation("Request {Method} {Path} executed in {ElapsedTime} ms.", method, path, elapsedTimeMs);

        // Save the elapsed time in HttpContext.Items for access by subsequent middleware.
        context.Items["ElapsedTime"] = elapsedTimeMs;
    }
}