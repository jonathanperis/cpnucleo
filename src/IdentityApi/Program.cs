var builder = WebApplication.CreateSlimBuilder(args);

builder.ConfigureOpenTelemetry();

var allowedCorsOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>() is { Length: > 0 } configuredOrigins
        ? configuredOrigins
        : ["https://cpnucleo.jonathanperis.tech"];

builder.Services
    .AddAuthenticationJwtBearer(s => s.SigningKey = builder.Configuration["Jwt:SigningKey"] ?? throw new InvalidOperationException("Jwt:SigningKey configuration is missing."))
    .AddAuthorization();

builder.Services.Configure<Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerOptions>("Bearer", options =>
{
    options.MapInboundClaims = false;
    options.TokenValidationParameters.ValidateIssuer = true;
    options.TokenValidationParameters.ValidIssuer = builder.Configuration["Jwt:Issuer"];
    options.TokenValidationParameters.ValidateAudience = true;
    options.TokenValidationParameters.ValidAudience = builder.Configuration["Jwt:Audience"];
    options.TokenValidationParameters.ValidateLifetime = true;
    options.TokenValidationParameters.ClockSkew = TimeSpan.FromMinutes(1);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CpnucleoWebClient", policy =>
    {
        policy
            .WithOrigins(allowedCorsOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services
    .Configure<JwtCreationOptions>(o =>
    {
        o.SigningKey = builder.Configuration["Jwt:SigningKey"] ?? throw new InvalidOperationException("Jwt:SigningKey configuration is missing.");
        o.Issuer = builder.Configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer configuration is missing.");
        o.Audience = builder.Configuration["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience configuration is missing.");
    });

builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 10, // Allow 10 requests
                Window = TimeSpan.FromMinutes(1), // Per 1-minute window
                QueueLimit = 5, // Queue up to 5 additional requests
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst, // Process oldest requests first
                AutoReplenishment = true // Default: automatically replenish permits
            }));

    options.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        context.HttpContext.Response.ContentType = "text/plain";

        var retryAfterSeconds = context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter)
            ? Math.Ceiling(retryAfter.TotalSeconds).ToString()
            : "60";
        context.HttpContext.Response.Headers.RetryAfter = retryAfterSeconds;

        await context.HttpContext.Response.WriteAsync("Rate limit exceeded. Please try again later.", cancellationToken);

        var logger = context.HttpContext.RequestServices
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger("IdentityApi.RateLimiting");
        logger.LogWarning("Rate limit exceeded for IP: {IpAddress}",
            context.HttpContext.Connection.RemoteIpAddress);
    };
});

builder.Services.AddOutputCache(options =>
{
    options.AddBasePolicy(b => b.Expire(TimeSpan.FromSeconds(10)));
    options.AddBasePolicy(b => b.Cache());
});

builder.Services.AddHealthChecks();

builder.Services
    .AddFastEndpoints()
    .SwaggerDocument(o =>
    {
        o.EnableJWTBearerAuth = true;
        o.ShortSchemaNames = true;
        o.AutoTagPathSegmentIndex = 1;
        o.TagDescriptions = tags =>
        {
            tags["Login"] = "Authenticate users and issue Cpnucleo access tokens.";
            tags["Refresh"] = "Refresh authenticated sessions and issued tokens.";
            tags["Register"] = "Register new Cpnucleo users.";
        };
        o.DocumentSettings = s =>
        {
            s.DocumentName = "v1";
            s.Title = "Cpnucleo Identity API";
            s.Description = "Authentication and authorization API for Cpnucleo users, tokens, and sessions.";
            s.Version = "v1";
            s.PostProcess = document =>
            {
                document.Info.Contact = new NSwag.OpenApiContact
                {
                    Name = "Cpnucleo Identity Support",
                    Url = "https://cpnucleo.jonathanperis.tech"
                };
                document.Info.License = new NSwag.OpenApiLicense
                {
                    Name = "MIT",
                    Url = "https://cpnucleo.jonathanperis.tech"
                };
                document.Info.TermsOfService = "https://cpnucleo.jonathanperis.tech";
            };
        };
    });

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseOutputCache();

app.Use(async (context, next) =>
{
    context.Response.Headers.TryAdd("Strict-Transport-Security", "max-age=31536000; includeSubDomains");
    context.Response.Headers.TryAdd("X-Content-Type-Options", "nosniff");
    context.Response.Headers.TryAdd("X-Frame-Options", "DENY");
    context.Response.Headers.TryAdd("Referrer-Policy", "strict-origin-when-cross-origin");
    if (context.Request.Path.StartsWithSegments("/swagger"))
        context.Response.Headers.TryAdd("Content-Security-Policy", "default-src 'self'; script-src 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline'; img-src 'self' data:; frame-ancestors 'none'");
    else context.Response.Headers.TryAdd("Content-Security-Policy", "default-src 'none'; frame-ancestors 'none'; base-uri 'none'");

    await next();

    if (context.Request.Path.Value?.Equals("/healthz", StringComparison.OrdinalIgnoreCase) == true)
    {
        app.Logger.LogInformation("GET /healthz {StatusCode}", context.Response.StatusCode);
    }
});

app.UseCors("CpnucleoWebClient");

app.UseHealthChecks("/healthz", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions { Predicate = _ => false });
app.UseHealthChecks("/readyz");

app.UseRateLimiter();

app.UseAuthentication();

app.Use(async (context, next) =>
{
    if (context.User.Identity?.IsAuthenticated == true &&
        !context.User.HasClaim(claim => claim.Type == CpnucleoClaimTypes.Subject && !string.IsNullOrWhiteSpace(claim.Value)))
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        await context.Response.WriteAsync("Authenticated tokens must include a subject claim.");
        return;
    }

    await next();
});

app.UseAuthorization();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseInfrastructure();
app.UseMiddleware<ElapsedTimeMiddleware>();
app.UseFastEndpoints(c => c.Endpoints.RoutePrefix = "api");

app.MapGet("/", () => "Hello World!");

app.UseSwaggerGen();

app.Run();
