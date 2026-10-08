var builder = WebApplication.CreateSlimBuilder(args);

if (args.Contains("--seed-lab") || args.Contains("--reset-lab"))
{
    if (!builder.Environment.IsDevelopment()) throw new InvalidOperationException("Lab seeding/reset is available only in Development.");
    await using var database = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseNpgsql(builder.Configuration["DB_CONNECTION_STRING"]).Options);
    if (args.Contains("--reset-lab")) await database.Database.EnsureDeletedAsync();
    await database.Database.MigrateAsync();
    await LabSeeder.SeedAsync(database, builder.Configuration["Seed:Profile"] ?? "tiny",
        builder.Configuration["CPNUCLEO_DEMO_PASSWORD"] ?? "LocalLearning@123");
    return;
}

if (args.Contains("--migrate-database", StringComparer.OrdinalIgnoreCase))
{
    await using var database = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseNpgsql(builder.Configuration["DB_CONNECTION_STRING"] ?? throw new InvalidOperationException("DB_CONNECTION_STRING is required.")).Options);
    await database.Database.MigrateAsync();
    return;
}

if (args.Contains("--run-fake-data-csv-import", StringComparer.OrdinalIgnoreCase))
{
    // The importer truncates every table before loading the demo dataset. It never runs in Production.
    if (builder.Environment.IsProduction())
    {
        throw new InvalidOperationException("The FakeData CSV import truncates all data and is disabled in Production.");
    }

    await FakeDataCsvImporter.RunAsync(
        builder.Configuration.GetValue<string>("DB_CONNECTION_STRING") ?? throw new InvalidOperationException("DB_CONNECTION_STRING configuration is missing."),
        new ConsoleSeedLogger("FakeDataCsvImporter"));
    return;
}

if (args.Contains("--generate-legacy-fake-data", StringComparer.OrdinalIgnoreCase))
{
    if (!builder.Environment.IsDevelopment()) throw new InvalidOperationException("Legacy fake data generation is available only in Development.");
    FakeDataHelper.CreateSqlCsvDumpFile();
    Console.WriteLine("Generated dml-data/*.csv and the COPY script. Move them into docker-entrypoint-initdb.d for the legacy compose.yaml dataset.");
    return;
}

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
        // Signing keys come from IdentityApi's discovery document (JWKS); none are configured here.
        options.MetadataAddress = JwtKeys.MetadataAddress(builder.Configuration);
        options.RequireHttpsMetadata = JwtKeys.RequireHttpsMetadata(builder.Configuration);
        options.TokenValidationParameters = JwtKeys.ValidationParameters(builder.Configuration);
        options.Events = new JwtBearerEvents
        {
            // Signature and lifetime are not enough: reject tokens whose account was deactivated,
            // whose credentials changed, or whose admin claim is no longer configured.
            OnTokenValidated = async context =>
            {
                var failure = await context.HttpContext.RequestServices.GetRequiredService<TokenSessionValidator>()
                    .ValidateAsync(context.Principal!, context.HttpContext.RequestAborted);
                if (failure is not null) context.Fail(failure);
            }
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserAdministration", policy => policy
        .RequireAuthenticatedUser()
        .RequireClaim(CpnucleoClaimTypes.Subject)
        .RequireClaim(CpnucleoClaimTypes.Admin, "true"));
});

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

builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 300, // A dashboard and CRUD forms issue several requests per screen
                Window = TimeSpan.FromMinutes(1), // Per 1-minute window
                QueueLimit = 20, // Queue up to 20 additional requests
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst, // Process oldest requests first
                AutoReplenishment = true // Default: automatically replenish permits
            }));

    // Global cap on concurrent password verifications (self-service password changes).
    options.AddConcurrencyLimiter(WebApi.Endpoints.Account.ChangePassword.Endpoint.ConcurrencyPolicy, limiter =>
    {
        limiter.PermitLimit = 4;
        limiter.QueueLimit = 16;
        limiter.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });

    options.OnRejected = (context, cancellationToken) => ApiErrorEnvelopeExtensions.WriteRateLimitRejectionAsync(
        context.HttpContext,
        context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter) ? retryAfter : null,
        "WebApi.RateLimiting",
        cancellationToken);
});

builder.Services.AddHealthChecks();
builder.Services.AddSingleton<ListingChangeNotifier>();
builder.Services.AddSingleton(_ => new PasswordChangeThrottle(TimeProvider.System));

builder.Services
    // Only this host's endpoints: other API assemblies loaded in the same process must not be mapped.
    .AddFastEndpoints(o =>
    {
        o.DisableAutoDiscovery = true;
        o.Assemblies = [typeof(WebApi.Endpoints.Project.CreateProject.Endpoint).Assembly];
    })
    .SwaggerDocument(o =>
    {
        o.EnableJWTBearerAuth = true;
        o.ShortSchemaNames = true;
        o.ExcludeNonFastEndpoints = true;
        // Endpoints tag themselves (WithTags); route-segment auto-tagging would add a second tag.
        o.AutoTagPathSegmentIndex = 0;
        o.TagDescriptions = tags =>
        {
            tags["Account"] = "The signed-in user's own profile and password.";
            tags["Appointments"] = "Manage appointments and scheduling records.";
            tags["Assignments"] = "Manage work assignments and ownership.";
            tags["AssignmentImpediments"] = "Track impediments attached to assignments.";
            tags["AssignmentTypes"] = "Manage assignment classification data.";
            tags["Impediments"] = "Manage project and workflow blockers.";
            tags["Organizations"] = "Manage tenant organizations.";
            tags["Projects"] = "Manage projects and project metadata.";
            tags["Users"] = "Manage users exposed by the Web API.";
            tags["UserAssignments"] = "Manage user-to-assignment relationships.";
            tags["UserProjects"] = "Manage user-to-project relationships.";
            tags["Workflows"] = "Manage workflow definitions and transitions.";
        };
        o.DocumentSettings = s =>
        {
            s.DocumentName = "v1";
            s.Title = "Cpnucleo Web API";
            s.Description = "Authenticated REST API for Cpnucleo project, workflow, assignment, organization, and user management.";
            s.Version = "v1";
            s.SchemaSettings.SchemaNameGenerator = new SchemaNameGenerator();
            s.OperationProcessors.Add(new PaginationQueryProcessor());
            s.OperationProcessors.Add(new ErrorResponsesProcessor());
            // Added to (not replacing) FastEndpoints' own post-processing, which emits the tag descriptions.
            s.PostProcess += document =>
            {
                document.Info.Contact = new NSwag.OpenApiContact
                {
                    Name = "Cpnucleo API Support",
                    Url = "https://cpnucleo.jonathanperis.tech"
                };
                document.Info.License = new NSwag.OpenApiLicense
                {
                    Name = "MIT",
                    Url = "https://cpnucleo.jonathanperis.tech"
                };
                document.Info.TermsOfService = "https://cpnucleo.jonathanperis.tech";
                // List operations describe pagination as flat query keys (PaginationQueryProcessor).
                document.Components.Schemas.Remove(nameof(PaginationParams));
            };
        };
    });

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

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

    if (context.Request.AcceptsServerSentEvents() && long.TryParse(context.User.FindFirst("exp")?.Value, out var expiresAt))
    {
        using var lifetime = CancellationTokenSource.CreateLinkedTokenSource(context.RequestAborted);
        var remaining = DateTimeOffset.FromUnixTimeSeconds(expiresAt) - DateTimeOffset.UtcNow;
        lifetime.CancelAfter(remaining > TimeSpan.Zero ? remaining : TimeSpan.Zero);
        var original = context.RequestAborted;
        context.RequestAborted = lifetime.Token;
        try { await next(); }
        finally { context.RequestAborted = original; }
        return;
    }

    await next();
});

app.UseAuthorization();
app.UseInfrastructure();
app.UseMiddleware<ElapsedTimeMiddleware>();
// FastEndpoints applies this configuration once per process, and the integration tests host WebApi
// and IdentityApi together: keep the block identical in both hosts (FastEndpointsConfigurationTests).
app.UseFastEndpoints(c =>
{
    c.Endpoints.RoutePrefix = "api";
    // Operation ids and route names are the feature folder (UpdateProject); every class is named Endpoint.
    c.Endpoints.NameGenerator = context => context.EndpointType.Namespace!.Split('.')[^1];
    // Validation failures use the same envelope, message and content type as every other error.
    c.Errors.ContentType = "application/json";
    c.Errors.ProducesMetadataType = typeof(ApiErrorResponse);
    c.Errors.ResponseBuilder = (failures, _, statusCode) =>
        ApiErrors.ValidationResponse(statusCode, failures.Select(failure => (failure.PropertyName, failure.ErrorMessage)));
});

app.MapGet("/", () => "Hello World!");

app.UseSwaggerGen();

app.Run();
