namespace WebApi.Middlewares;

public class ElapsedTimeMiddleware(RequestDelegate next, ILogger<ElapsedTimeMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ElapsedTimeMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        _logger.LogInformation("Request {Method} {Path} starting.", context.Request.Method, context.Request.Path);

        var stopwatch = Stopwatch.StartNew();

        await _next(context);

        stopwatch.Stop();

        var elapsedTimeMs = stopwatch.Elapsed.TotalMilliseconds;

        _logger.LogInformation("Request {Method} {Path} executed in {ElapsedTime} ms.", context.Request.Method, context.Request.Path, elapsedTimeMs);

        // Save the elapsed time in HttpContext.Items for access by subsequent middleware.
        context.Items["ElapsedTime"] = elapsedTimeMs;
    }
}