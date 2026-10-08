using IdentityApi.Oidc;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Options;
using Open.IdentityServer.Services;
using Open.IdentityServer.Stores;

var builder = WebApplication.CreateSlimBuilder(args);

builder.ConfigureOpenTelemetry();

builder.Services.AddCors(options =>
{
    // Cors:AllowedOrigins (default https://cpnucleo.jonathanperis.tech): the WebClient origins.
    options.AddPolicy("CpnucleoWebClient", policy =>
    {
        policy
            .WithOrigins(OidcSettings.WebClientOrigins(builder.Configuration))
            .AllowAnyHeader()
            .AllowAnyMethod()
            // Lets the browser client read how long to wait after a 429.
            .WithExposedHeaders("Retry-After");
    });
});

// IdentityApi reads accounts on behalf of the system; it exposes no resource CRUD.
builder.Services.AddSingleton<Application.Common.Security.ICurrentUser>(Application.Common.Security.StaticCurrentUser.System);
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddSingleton<LoginThrottle>();
builder.Services.AddSingleton<TimingSafePasswordCheck>();
builder.Services.AddSingleton<IdentityKeyProtector>();
builder.Services.AddSingleton<AccountDirectory>();
builder.Services.AddSingleton<IdentitySessions>();
builder.Services.AddSingleton<SigningKeyRing>();
builder.Services.AddHostedService<IdentityMaintenance>();

// OpenID Connect server: authorization code + PKCE for the WebClient, client credentials for
// service clients, one-time refresh tokens, revocation, introspection and end session.
builder.Services
    .AddIdentityServer(options =>
    {
        options.IssuerUri = JwtKeys.Issuer(builder.Configuration);
        // The sign-in UI is the WebClient's Astro page; errors are explained there too. Sign-in goes
        // through a local step that picks the page of the WebClient origin that started the request.
        options.UserInteraction.LoginUrl = IdentityApi.Endpoints.Account.LoginPage.Endpoint.Route;
        options.UserInteraction.LoginReturnUrlParameter = "authRequest";
        options.UserInteraction.ErrorUrl = OidcSettings.LoginPageUrl(builder.Configuration);
        options.UserInteraction.ErrorIdParameter = "errorId";
        options.UserInteraction.LogoutUrl = "/api/account/logout";
        options.Authentication.CookieLifetime = OidcSettings.MaximumSessionLength;
        options.Authentication.CookieSlidingExpiration = false;
        options.Authentication.CookieSameSiteMode = SameSiteMode.Lax;
        // The WebClient is same-site (subdomain or localhost port): Lax works over the lab's plain HTTP too.
        options.Authentication.CheckSessionCookieSameSiteMode = SameSiteMode.Lax;
        // No device flow. The session-check endpoint stays on: it is what stamps every token with the
        // session id (sid) that sign-out revokes.
        options.Endpoints.EnableDeviceAuthorizationEndpoint = false;
        options.Events.RaiseErrorEvents = true;
        options.Events.RaiseFailureEvents = true;
    })
    .AddInMemoryIdentityResources(OidcSettings.IdentityResources())
    .AddInMemoryApiScopes(OidcSettings.ApiScopes())
    .AddInMemoryApiResources([])
    .AddInMemoryClients([])
    .AddProfileService<CpnucleoProfileService>()
    .AddPersistedGrantStore<PersistedGrantStore>()
    .AddCustomTokenRequestValidator<CpnucleoTokenRequestValidator>();

// Clients and API resources come from configuration when first used, after every source is loaded.
builder.Services.AddSingleton<IEnumerable<Open.IdentityServer.Models.Client>>(services =>
    OidcSettings.Clients(services.GetRequiredService<IConfiguration>()).ToList());
builder.Services.AddSingleton<IEnumerable<Open.IdentityServer.Models.ApiResource>>(services =>
    OidcSettings.ApiResources(services.GetRequiredService<IConfiguration>()).ToList());

// Rotating signing keys published as JWKS; API hosts read them from the discovery document.
builder.Services.AddSingleton<ISigningCredentialStore>(services => services.GetRequiredService<SigningKeyRing>());
builder.Services.AddSingleton<IValidationKeysStore>(services => services.GetRequiredService<SigningKeyRing>());
builder.Services.AddTransient<IRefreshTokenService, ReuseDetectingRefreshTokenService>();

// Identity cookies survive restarts: Data Protection keys live in PostgreSQL, encrypted.
builder.Services.AddDataProtection().SetApplicationName("cpnucleo-identity");
builder.Services.AddSingleton<DataProtectionKeyStore>();
builder.Services.AddSingleton<IConfigureOptions<KeyManagementOptions>>(services => new ConfigureOptions<KeyManagementOptions>(options =>
{
    options.XmlRepository = services.GetRequiredService<DataProtectionKeyStore>();
    options.XmlEncryptor = new KeyEncryptionXmlEncryptor(services.GetRequiredService<IdentityKeyProtector>());
}));

builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                // A sign-in takes four requests (authorize, form post, callback, token) and every tab
                // refreshes on its own; brute force is bounded per login by LoginThrottle.
                PermitLimit = 60,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 5,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                AutoReplenishment = true
            }));

    // Global cap on concurrent password verifications, independent of client addresses.
    options.AddConcurrencyLimiter(IdentityApi.Endpoints.Account.Login.Endpoint.ConcurrencyPolicy, limiter =>
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

builder.Services.AddAuthorization();
builder.Services.AddHealthChecks();

builder.Services
    // Only this host's endpoints, validators and groups, listed at compile time by FastEndpoints.Generator
    // (no assembly scanning): other API assemblies loaded in the same process must not be mapped.
    .AddFastEndpoints(IdentityApi.DiscoveredTypes.All)
    .SwaggerDocument(o =>
    {
        o.EnableJWTBearerAuth = false;
        o.ShortSchemaNames = true;
        o.ExcludeNonFastEndpoints = true;
        // Endpoints tag themselves (WithTags); route-segment auto-tagging would add a second tag.
        o.AutoTagPathSegmentIndex = 0;
        o.TagDescriptions = tags =>
        {
            tags["Account"] = "Sign-in and sign-out steps of the OpenID Connect flows. Protocol endpoints are listed in /.well-known/openid-configuration.";
        };
        o.DocumentSettings = s =>
        {
            s.DocumentName = "v1";
            s.SchemaSettings.SchemaNameGenerator = new IdentityApi.ServiceExtensions.SchemaNameGenerator();
            s.Title = "Cpnucleo Identity API";
            s.Description = "OpenID Connect provider for Cpnucleo: authorization code with PKCE, client credentials, refresh token rotation, revocation and end session.";
            s.Version = "v1";
            // Added to (not replacing) FastEndpoints' own post-processing, which emits the tag descriptions.
            s.PostProcess += document =>
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

// Protocol endpoints (/connect/*, /.well-known/*) and the identity cookie authentication.
app.UseIdentityServer();
app.UseAuthorization();
// No UseInfrastructure(): its Delta ETags are for per-caller data listings, which this host doesn't serve.
app.UseMiddleware<ElapsedTimeMiddleware>();
// FastEndpoints applies this configuration once per process, and the integration tests host WebApi
// and IdentityApi together: keep the block identical in both hosts (FastEndpointsConfigurationTests).
app.UseFastEndpoints(c =>
{
    c.Endpoints.RoutePrefix = "api";
    // Build validators, binders and compiled delegates at startup, before /readyz reports ready.
    c.Endpoints.Warmup();
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
