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
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        context.HttpContext.Response.ContentType = "text/plain";

        var retryAfterSeconds = context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter)
            ? Math.Ceiling(retryAfter.TotalSeconds).ToString()
            : "60";
        context.HttpContext.Response.Headers.RetryAfter = retryAfterSeconds;

        await context.HttpContext.Response.WriteAsync("Rate limit exceeded. Please try again later.", cancellationToken);

        var logger = context.HttpContext.RequestServices
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger("WebApi.RateLimiting");
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
        o.EnableJWTBearerAuth = true;
        o.ShortSchemaNames = true;
        o.AutoTagPathSegmentIndex = 1;
        o.TagDescriptions = tags =>
        {
            tags["Appointment"] = "Manage appointments and scheduling records.";
            tags["Assignment"] = "Manage work assignments and ownership.";
            tags["AssignmentImpediment"] = "Track impediments attached to assignments.";
            tags["AssignmentType"] = "Manage assignment classification data.";
            tags["Impediment"] = "Manage project and workflow blockers.";
            tags["Organization"] = "Manage tenant organizations.";
            tags["Project"] = "Manage projects and project metadata.";
            tags["User"] = "Manage users exposed by the Web API.";
            tags["UserAssignment"] = "Manage user-to-assignment relationships.";
            tags["UserProject"] = "Manage user-to-project relationships.";
            tags["Workflow"] = "Manage workflow definitions and transitions.";
        };
        o.DocumentSettings = s =>
        {
            s.DocumentName = "v1";
            s.Title = "Cpnucleo Web API";
            s.Description = "Authenticated REST API for Cpnucleo project, workflow, assignment, organization, and user management.";
            s.Version = "v1";
            s.SchemaSettings.SchemaNameGenerator = new SchemaNameGenerator();
            s.PostProcess = document =>
            {
                document.Info.Contact = new NSwag.OpenApiContact
                {
                    Name = "Cpnucleo API Support",
                    Url = "https://cpnucleo.jonathanperis.tech"
                };
                document.Info.License = new NSwag.OpenApiLicense
                {
                    Name = "Proprietary",
                    Url = "https://cpnucleo.jonathanperis.tech"
                };
                document.Info.TermsOfService = "https://cpnucleo.jonathanperis.tech";
            };
        };
    });

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseHealthChecks("/healthz");

app.UseInfrastructure();

app.UseRateLimiter();

app.UseCors("CpnucleoWebClient")
    .UseAuthentication()
    .UseAuthorization()
    .UseFastEndpoints(c => c.Endpoints.RoutePrefix = "api")
    .UseMiddleware<ElapsedTimeMiddleware>()
    .UseMiddleware<ErrorHandlingMiddleware>();

app.MapGet("/", () => "Hello World!");

app.UseSwaggerGen();

app.Run();
