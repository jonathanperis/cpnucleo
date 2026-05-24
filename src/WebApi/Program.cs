var builder = WebApplication.CreateSlimBuilder(args);

var logger = LoggerFactory.Create(logging =>
{
    logging.AddConsole();
}).CreateLogger<Program>();

builder.ConfigureOpenTelemetry();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SigningKey"] ?? throw new InvalidOperationException("Jwt:SigningKey configuration is missing."))),
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer configuration is missing."),
            ValidAudience = builder.Configuration["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience configuration is missing."),
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ClockSkew = TimeSpan.FromMinutes(1)
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 50, // Allow 50 requests
                Window = TimeSpan.FromMinutes(1), // Per 1-minute window
                QueueLimit = 10, // Queue up to 10 additional requests
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

builder.Services.AddHealthChecks();

builder.Services
    // .AddFastEndpoints(o => o.SourceGeneratorDiscoveredTypes = WebApi.DiscoveredTypes.All)
    .AddFastEndpoints()
    .SwaggerDocument(o =>
    {
        o.DocumentSettings = s =>
        {
            s.Title = "Cpnucleo Web API";
            s.Description = "A sample project that implements best practices when building modern .NET projects";
            s.Version = "v1";
            s.SchemaSettings.SchemaNameGenerator = new SchemaNameGenerator();
        };
        o.AutoTagPathSegmentIndex = 0; // Disable the auto-tagging by setting the AutoTagPathSegmentIndex property to 0
    });

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseHealthChecks("/healthz");

app.UseInfrastructure();

app.UseAuthentication()
    .UseAuthorization()
    .UseFastEndpoints(c => c.Endpoints.RoutePrefix = "api")
    .UseMiddleware<ElapsedTimeMiddleware>()
    .UseMiddleware<ErrorHandlingMiddleware>();

app.MapGet("/", () => "Hello World!");

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerGen();
}

app.MapApiClientEndpoint("/cs-client", c =>
    {
        c.SwaggerDocumentName = "v1";
        c.Language = GenerationLanguage.CSharp;
        c.ClientNamespaceName = "Cpnucleo.WebApi.Client";
        c.ClientClassName = "WebApiClient";
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
        c.OutputPath = Path.Join(app.Environment.WebRootPath, "ApiClients", "CSharp");
        c.ClientNamespaceName = "Cpnucleo.WebApi.Client";
        c.ClientClassName = "WebApiClient";
        c.CreateZipArchive = true; //if you'd like a zip file as well
    },
    c =>
    {
        c.SwaggerDocumentName = "v1";
        c.Language = GenerationLanguage.TypeScript;
        c.OutputPath = Path.Join(app.Environment.WebRootPath, "ApiClients", "Typescript");
        c.ClientNamespaceName = "Cpnucleo.WebApi.Client";
        c.ClientClassName = "cpnucleo-webapi-client";
    });

app.Run();
