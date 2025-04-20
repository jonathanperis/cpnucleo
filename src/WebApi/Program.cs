var builder = WebApplication.CreateSlimBuilder(args);

var logger = LoggerFactory.Create(builder =>
{
    // _ = builder.AddApplicationInsights();
}).CreateLogger<Program>();

builder.ConfigureOpenTelemetry();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey("ForTheLoveOfGodStoreAndLoadThisSecurely"u8.ToArray()),
            ValidIssuer = "https://identity.peris-studio.dev",
            ValidAudience = "https://peris-studio.dev",
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidateIssuer = true,
            ValidateAudience = true
        };
    });

builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: partition => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 50, // Allow 50 requests
                Window = TimeSpan.FromMinutes(1), // Per 1-minute window
                QueueLimit = 10, // Queue up to 10 additional requests
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst, // Process oldest requests first
                AutoReplenishment = true, // Default: automatically replenish permits
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

// builder.Services.AddOutputCache(options =>
// {
//     options.AddBasePolicy(builder => builder.Expire(TimeSpan.FromSeconds(10)));
//     options.AddBasePolicy(builder => builder.Cache());
// });

builder.Services.AddHealthChecks();

builder.Services
    .AddFastEndpoints()
    .SwaggerDocument(o =>
    {
        o.DocumentSettings = s =>
        {
            s.Title = "Cpnucleo Web API";
            s.Description = "A sample project that implements the best praticles when building modern .NET projects";
            s.Version = "v1";
        };
    });

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{

}

// app.UseOutputCache();

app.UseInfrastructure();

app.
    UseFastEndpoints()
    .UseMiddleware<ElapsedTimeMiddleware>()
    .UseMiddleware<ErrorHandlingMiddleware>()
    .UseSwaggerGen();

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
        c.ClientNamespaceName = "MyCompanyName";
        c.ClientClassName = "MyCsClient";
        c.CreateZipArchive = true; //if you'd like a zip file as well
    },
    c =>
    {
        c.SwaggerDocumentName = "v1";
        c.Language = GenerationLanguage.TypeScript;
        c.OutputPath = Path.Combine(app.Environment.WebRootPath, "ApiClients", "Typescript");
        c.ClientNamespaceName = "MyCompanyName";
        c.ClientClassName = "MyTsClient";
    });

app.Run();