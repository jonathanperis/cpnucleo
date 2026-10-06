using Microsoft.AspNetCore.Http;

namespace Infrastructure.Http;

public static class ApiErrorEnvelopeExtensions
{
    /// <summary>
    /// Gives every error response the <see cref="ApiErrorResponse"/> shape: exceptions are translated
    /// by <see cref="ApiExceptionMiddleware"/> and body-less error statuses (401/403/404 from
    /// authentication, authorization or endpoints) receive a default message.
    /// </summary>
    public static IApplicationBuilder UseApiErrorEnvelope(this IApplicationBuilder app)
    {
        app.UseStatusCodePages(context =>
        {
            var response = context.HttpContext.Response;
            if (response.StatusCode < 400 || response.StatusCode == 499 || response.ContentLength > 0 || response.ContentType is not null)
                return Task.CompletedTask;
            return ApiErrors.WriteAsync(context.HttpContext, response.StatusCode, cancellationToken: context.HttpContext.RequestAborted);
        });
        app.UseMiddleware<ApiExceptionMiddleware>();
        return app;
    }

    /// <summary>A rate-limiter rejection callback that writes the envelope and Retry-After.</summary>
    public static async ValueTask WriteRateLimitRejectionAsync(HttpContext context, TimeSpan? retryAfter, string loggerCategory, CancellationToken cancellationToken)
    {
        var seconds = Math.Max(1, (int)Math.Ceiling((retryAfter ?? TimeSpan.FromSeconds(60)).TotalSeconds));
        context.Response.Headers.RetryAfter = seconds.ToString(CultureInfo.InvariantCulture);
        await ApiErrors.WriteAsync(context, StatusCodes.Status429TooManyRequests,
            $"Rate limit exceeded. Please try again in {seconds} seconds.", cancellationToken: cancellationToken);

        context.RequestServices.GetRequiredService<ILoggerFactory>()
            .CreateLogger(loggerCategory)
            .LogWarning("Rate limit exceeded for {Path}.", context.Request.Path);
    }
}
