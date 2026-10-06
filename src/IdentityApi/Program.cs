var builder = WebApplication.CreateSlimBuilder(args);

builder.ConfigureOpenTelemetry();

var allowedCorsOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>() is { Length: > 0 } configuredOrigins
        ? configuredOrigins
        : ["https://cpnucleo.jonathanperis.tech"];

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = JwtKeys.ValidationParameters(builder.Configuration);
    });
builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CpnucleoWebClient", policy =>
    {
        policy
            .WithOrigins(allowedCorsOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            // Lets the browser client read how long to wait after a 429.
            .WithExposedHeaders("Retry-After");
    });
});

// IdentityApi reads accounts on behalf of the system (login, refresh); it exposes no resource CRUD.
builder.Services.AddSingleton<Application.Common.Security.ICurrentUser>(Application.Common.Security.StaticCurrentUser.System);
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddSingleton<TokenIssuer>();
builder.Services.AddSingleton<LoginThrottle>();
builder.Services.AddSingleton<TimingSafePasswordCheck>();

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

    // Global cap on concurrent password verifications, independent of client addresses.
    options.AddConcurrencyLimiter(IdentityApi.Endpoints.Login.Endpoint.ConcurrencyPolicy, limiter =>
    {
        limiter.PermitLimit = 4;
        limiter.QueueLimit = 16;
        limiter.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });

    options.OnRejected = (context, cancellationToken) => ApiErrorEnvelopeExtensions.WriteRateLimitRejectionAsync(
        context.HttpContext,
        context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter) ? retryAfter : null,
        "IdentityApi.RateLimiting",
        cancellationToken);
});

builder.Services.AddOutputCache(options =>
{
    options.AddBasePolicy(b => b.Expire(TimeSpan.FromSeconds(10)));
    options.AddBasePolicy(b => b.Cache());
});

builder.Services.AddHealthChecks();

builder.Services
    // Only this host's endpoints: other API assemblies loaded in the same process must not be mapped.
    .AddFastEndpoints(o =>
    {
        o.DisableAutoDiscovery = true;
        o.Assemblies = [typeof(IdentityApi.Endpoints.Login.Endpoint).Assembly];
    })
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
// Traefik routes this host directly (one proxy hop); WebApi sits behind Traefik and NGINX (two).
builder.Services.Configure<Microsoft.AspNetCore.Builder.ForwardedHeadersOptions>(options => options.ForwardLimit = 1);

var app = builder.Build();

app.UseOutputCache();

app.Use(async (context, next) =>
{
    // Added when the response starts, so error responses written after a Response.Clear() keep them.
    context.Response.OnStarting(() =>
    {
        context.Response.Headers.TryAdd("Strict-Transport-Security", "max-age=31536000; includeSubDomains");
        context.Response.Headers.TryAdd("X-Content-Type-Options", "nosniff");
        context.Response.Headers.TryAdd("X-Frame-Options", "DENY");
        context.Response.Headers.TryAdd("Referrer-Policy", "strict-origin-when-cross-origin");
        if (context.Request.Path.StartsWithSegments("/swagger"))
            context.Response.Headers.TryAdd("Content-Security-Policy", "default-src 'self'; script-src 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline'; img-src 'self' data:; frame-ancestors 'none'");
        else context.Response.Headers.TryAdd("Content-Security-Policy", "default-src 'none'; frame-ancestors 'none'; base-uri 'none'");
        return Task.CompletedTask;
    });

    await next();

    if (context.Request.Path.Value?.Equals("/healthz", StringComparison.OrdinalIgnoreCase) == true)
    {
        app.Logger.LogInformation("GET /healthz {StatusCode}", context.Response.StatusCode);
    }
});

app.UseCors("CpnucleoWebClient");
app.UseApiErrorEnvelope();

app.UseHealthChecks("/healthz", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions { Predicate = _ => false });
app.UseHealthChecks("/readyz");

app.UseRateLimiter();

app.UseAuthentication();

app.Use(async (context, next) =>
{
    if (context.User.Identity?.IsAuthenticated == true &&
        !context.User.HasClaim(claim => claim.Type == CpnucleoClaimTypes.Subject && !string.IsNullOrWhiteSpace(claim.Value)))
    {
        await ApiErrors.WriteAsync(context, StatusCodes.Status401Unauthorized, "Authenticated tokens must include a subject claim.");
        return;
    }

    await next();
});

app.UseAuthorization();
app.UseInfrastructure();
app.UseMiddleware<ElapsedTimeMiddleware>();
app.UseFastEndpoints(c => c.Endpoints.RoutePrefix = "api");

app.MapGet("/", () => "Hello World!");

app.UseSwaggerGen();

app.Run();
