var builder = WebApplication.CreateSlimBuilder(args);

var logger = LoggerFactory.Create(logging =>
{
    _ = logging.AddApplicationInsights();
}).CreateLogger<Program>();

builder.ConfigureOpenTelemetry();
builder.Logging.AddApplicationInsights();

builder.Services
    .AddAuthenticationJwtBearer(s => s.SigningKey = "ForTheLoveOfGodStoreAndLoadThisSecurely")
    .AddAuthorization() 
    .AddFastEndpoints();

builder.Services
    .Configure<JwtCreationOptions>(o =>
    {
        o.ExpireAt = DateTime.UtcNow.AddDays(1);
        o.SigningKey = "ForTheLoveOfGodStoreAndLoadThisSecurely";
        o.Issuer = "https://identity.peris-studio.dev";
        o.Audience = "https://peris-studio.dev";
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
                QueueLimit = 3, // Queue up to 10 additional requests
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst, // Process oldest requests first
                AutoReplenishment = true // Default: automatically replenish permits
            }));

    options.OnRejected = async (context, cancellationToken) =>
    {
        // Custom rejection handling logic
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        context.HttpContext.Response.Headers.RetryAfter = "60";

        await context.HttpContext.Response.WriteAsync("Rate limit exceeded. Please try again later.", cancellationToken);

        // Optional logging
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
    // .AddFastEndpoints(o => o.SourceGeneratorDiscoveredTypes = WebApi.DiscoveredTypes.All)
    .AddFastEndpoints()
    .SwaggerDocument(o =>
    {
        o.DocumentSettings = s =>
        {
            s.Title = "Cpnucleo Identity API";
            s.Description = "API for managing user authentication and authorization.";
            s.Version = "v1";
        };
        o.AutoTagPathSegmentIndex = 0; // Disable the auto-tagging by setting the AutoTagPathSegmentIndex property to 0
    });

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseOutputCache();

app.UseInfrastructure();

app.UseAuthentication()
    .UseAuthorization()
    .UseFastEndpoints()
        .UseMiddleware<ElapsedTimeMiddleware>()
        .UseMiddleware<ErrorHandlingMiddleware>();

app.MapHealthChecks("/healthz");
app.MapGet("/", () => "Hello World!");

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerGen();
}

app.MapApiClientEndpoint("/cs-client", c =>
    {
        c.SwaggerDocumentName = "v1";
        c.Language = GenerationLanguage.CSharp;
        c.ClientNamespaceName = "MyCompanyName";
        c.ClientClassName = "MyCsClient";
    },
    o =>
    {
        o.CacheOutput(p => p.Expire(TimeSpan.FromDays(365))); //cache the zip
        o.ExcludeFromDescription();
    });

await app.GenerateApiClientsAndExitAsync(
    c =>
    {
        c.SwaggerDocumentName = "v1"; //must match doc name above
        c.Language = GenerationLanguage.CSharp;
        c.OutputPath = Path.Combine(app.Environment.WebRootPath, "ApiClients", "CSharp");
        c.ClientNamespaceName = "Cpnucleo.WebApi.Client";
        c.ClientClassName = "Cpnucleo.WebApi.Client";
        c.CreateZipArchive = true; //if you'd like a zip file as well
    },
    c =>
    {
        c.SwaggerDocumentName = "v1";
        c.Language = GenerationLanguage.TypeScript;
        c.OutputPath = Path.Combine(app.Environment.WebRootPath, "ApiClients", "Typescript");
        c.ClientNamespaceName = "Cpnucleo.WebApi.Client";
        c.ClientClassName = "cpnucleo-webapi-client";
    });

app.Run();