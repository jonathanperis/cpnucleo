extern alias WebApiHost;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.RateLimiting;
using Dapper;
using Domain.Common.Security;
using Domain.Entities;
using Grpc.Core;
using Infrastructure.Common.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using Testcontainers.PostgreSql;

namespace WebApi.Integration.Tests.Hosts;

/// <summary>
/// Real WebApi, GrpcServer and IdentityApi hosts over one disposable PostgreSQL container with all
/// migrations applied. IdentityApi signs with a pinned test RSA key; the API hosts download it from
/// IdentityApi's discovery document (JWKS) exactly as in production, so tests can also mint tokens
/// for edge cases with the same key. Seeded accounts cover the three access levels: administrator,
/// project member and outsider (an active user with no memberships).
/// </summary>
public sealed class WebAppFixture : IAsyncLifetime
{
    public const string Issuer = "https://identity.integration.test";
    public const string Audience = "https://api.integration.test";
    public const string GrpcAudience = "https://grpc.integration.test";
    public const string WebOrigin = "https://web.integration.test";
    public const string KeyEncryptionSecret = "disposable-integration-key-encryption-secret";
    private const string MetadataAddress = "http://localhost/.well-known/openid-configuration";

    /// <summary>The identity server's signing key (pinned through <c>Jwt:SigningPrivateKey</c>).</summary>
    public static readonly RSA SigningKey = RSA.Create(2048);
    public static readonly string SigningKeyId = IdentityApi.Oidc.SigningKeyRing.KeyIdFor(SigningKey.ExportParameters(false));

    public static readonly TestAccount Admin = TestAccount.Create("admin", isAdmin: true);
    public static readonly TestAccount Member = TestAccount.Create("member");
    public static readonly TestAccount Outsider = TestAccount.Create("outsider");

    private readonly PostgreSqlContainer database = new PostgreSqlBuilder("postgres:16.15")
        .WithCommand("-c", "track_commit_timestamp=on").Build();
    private WebApplicationFactory<WebApiHost::Program> factory = null!;
    private WebApplicationFactory<GrpcServer.Handlers.Project.CreateProjectHandler> grpcFactory = null!;
    private WebApplicationFactory<IdentityApi.Security.LoginThrottle> identityFactory = null!;
    public HttpClient Client { get; private set; } = null!;
    private readonly System.Collections.Concurrent.ConcurrentQueue<string> failures = new();
    public string FailureDetails => string.Join("\n", failures);
    public string ConnectionString => database.GetConnectionString();

    public async ValueTask InitializeAsync()
    {
        await database.StartAsync();
        await using (var context = CreateDbContext())
        {
            await context.Database.MigrateAsync();
        }

        await using (var connection = new NpgsqlConnection(ConnectionString))
        {
            foreach (var account in new[] { Admin, Member, Outsider })
            {
                await connection.ExecuteAsync("""
                    INSERT INTO "Users" ("Id", "Name", "Login", "Password", "Salt", "CreatedAt", "Active")
                    VALUES (@Id, @Login, @Login, @PasswordHash, '', now(), true)
                    """, account);
            }
        }

        // The API hosts read signing keys from this host, so it is started (and its key set served)
        // before any of them handles a request, as a separately deployed IdentityApi would be.
        identityFactory = CreateIdentityFactory(disableRateLimiting: true);
        using (var warmUp = identityFactory.CreateClient())
            (await warmUp.GetAsync("/.well-known/openid-configuration/jwks")).EnsureSuccessStatusCode();

        factory = Track(new WebApplicationFactory<WebApiHost::Program>()).WithWebHostBuilder(builder =>
        {
            ConfigureApiHost(builder, Audience);
            builder.ConfigureServices(DisableRateLimiting);
        });
        Client = factory.CreateClient();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CreateToken());

        grpcFactory = Track(new WebApplicationFactory<GrpcServer.Handlers.Project.CreateProjectHandler>()).WithWebHostBuilder(builder =>
        {
            ConfigureApiHost(builder, GrpcAudience);
            builder.ConfigureServices(DisableRateLimiting);
        });
        grpcFactory.Services.MapRemoteCore("http://localhost", connection =>
        {
            connection.ChannelOptions.HttpHandler = grpcFactory.Server.CreateHandler();
            RegisterAllCommands(connection);
        });
    }

    // WithWebHostBuilder returns a derived factory; the root factories are disposed with the fixture.
    private readonly List<IAsyncDisposable> rootFactories = [];

    private T Track<T>(T rootFactory) where T : IAsyncDisposable
    {
        rootFactories.Add(rootFactory);
        return rootFactory;
    }

    public ApplicationDbContext CreateDbContext() => new(new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseNpgsql(ConnectionString).Options);

    public NpgsqlConnection CreateConnection() => new(ConnectionString);

    /// <summary>WebApi and IdentityApi hosts with extra configuration (e.g. no admin list).</summary>
    public (WebApplicationFactory<WebApiHost::Program> Api, WebApplicationFactory<IdentityApi.Security.LoginThrottle> Identity) CreateHosts(
        IReadOnlyDictionary<string, string?> overrides)
    {
        void Override(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((_, config) => config.AddInMemoryCollection(overrides));
            builder.ConfigureServices(DisableRateLimiting);
        }

        return (Track(new WebApplicationFactory<WebApiHost::Program>()).WithWebHostBuilder(builder =>
            {
                ConfigureApiHost(builder, Audience);
                Override(builder);
            }),
            Track(new WebApplicationFactory<IdentityApi.Security.LoginThrottle>()).WithWebHostBuilder(builder =>
            {
                ConfigureIdentityHost(builder);
                Override(builder);
            }));
    }

    /// <summary>A GrpcServer host with extra configuration, for transport-specific checks.</summary>
    public WebApplicationFactory<GrpcServer.Handlers.Project.CreateProjectHandler> CreateGrpcHost() =>
        Track(new WebApplicationFactory<GrpcServer.Handlers.Project.CreateProjectHandler>()).WithWebHostBuilder(builder =>
        {
            ConfigureApiHost(builder, GrpcAudience);
            builder.ConfigureServices(DisableRateLimiting);
        });

    /// <summary>A WebApi host with production rate limiting, for quota tests.</summary>
    public WebApplicationFactory<WebApiHost::Program> CreateRateLimitedWebApiFactory() =>
        Track(new WebApplicationFactory<WebApiHost::Program>()).WithWebHostBuilder(builder =>
        {
            ConfigureApiHost(builder, Audience);
            builder.ConfigureServices(CaptureFailures);
        });

    /// <summary>An IdentityApi host on the same database. Tests that exercise quotas get their own instance.</summary>
    public WebApplicationFactory<IdentityApi.Security.LoginThrottle> CreateIdentityFactory(bool disableRateLimiting) =>
        Track(new WebApplicationFactory<IdentityApi.Security.LoginThrottle>()).WithWebHostBuilder(builder =>
        {
            ConfigureIdentityHost(builder);
            if (disableRateLimiting) builder.ConfigureServices(DisableRateLimiting);
            else builder.ConfigureServices(CaptureFailures);
        });

    /// <summary>A browser-like identity client: cookies kept, redirects returned to the test.</summary>
    public HttpClient CreateIdentityClient() => CreateBrowserClient(identityFactory);

    public static HttpClient CreateBrowserClient<T>(WebApplicationFactory<T> identity) where T : class =>
        identity.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false, HandleCookies = true, BaseAddress = new Uri("https://localhost") });

    private void ConfigureShared(IWebHostBuilder builder, IDictionary<string, string?> hostValues)
    {
        builder.UseEnvironment("Testing");
        var values = new Dictionary<string, string?>
        {
            ["DB_CONNECTION_STRING"] = ConnectionString,
            ["Jwt:Issuer"] = Issuer,
            ["CPNUCLEO_ADMIN_LOGINS"] = Admin.Login,
            // Short window so revocation tests don't wait the production 30 seconds.
            ["Auth:SessionValidationCacheSeconds"] = "1"
        };
        foreach (var (key, value) in hostValues) values[key] = value;
        builder.ConfigureAppConfiguration((_, config) => config.AddInMemoryCollection(values));
    }

    private void ConfigureIdentityHost(IWebHostBuilder builder) => ConfigureShared(builder, new Dictionary<string, string?>
    {
        ["Jwt:Audience"] = Audience,
        ["Jwt:SigningPrivateKey"] = SigningKey.ExportPkcs8PrivateKeyPem(),
        ["Identity:GrpcAudience"] = GrpcAudience,
        ["Identity:KeyEncryptionSecret"] = KeyEncryptionSecret,
        ["Cors:AllowedOrigins:0"] = WebOrigin
    });

    /// <summary>
    /// An API host that validates tokens against the identity host's discovery document, fetched
    /// through the in-memory test server instead of the network.
    /// </summary>
    private void ConfigureApiHost(IWebHostBuilder builder, string audience)
    {
        ConfigureShared(builder, new Dictionary<string, string?>
        {
            ["Jwt:Audience"] = audience,
            ["Jwt:MetadataAddress"] = MetadataAddress
        });
        builder.ConfigureTestServices(services => services.PostConfigure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            options.ConfigurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                MetadataAddress,
                new OpenIdConnectConfigurationRetriever(),
                new HttpDocumentRetriever(new HttpClient(new IdentityServerHandler(() => identityFactory))) { RequireHttps = false });
        }));
    }

    /// <summary>Forwards discovery requests to the identity test server, created lazily.</summary>
    private sealed class IdentityServerHandler(Func<WebApplicationFactory<IdentityApi.Security.LoginThrottle>> identity) : HttpMessageHandler
    {
        private HttpMessageInvoker? invoker;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) =>
            (invoker ??= new HttpMessageInvoker(identity().Server.CreateHandler())).SendAsync(request, cancellationToken);
    }

    private void CaptureFailures(IServiceCollection services) =>
        services.AddLogging(logging => logging.AddProvider(new FailureLoggerProvider(failures))
            .AddFilter<FailureLoggerProvider>((_, level) => level >= LogLevel.Error));

    private void DisableRateLimiting(IServiceCollection services)
    {
        CaptureFailures(services);
        // CRUD tests exercise auth and real persistence; quota behavior has its own tests and hosts.
        services.PostConfigure<RateLimiterOptions>(options => options.GlobalLimiter =
            PartitionedRateLimiter.Create<HttpContext, string>(_ => RateLimitPartition.GetNoLimiter("integration")));
    }

    private static void RegisterAllCommands(object connection)
    {
        var register = connection.GetType().GetMethods()
            .Single(method => method.Name == "Register" && method.IsGenericMethodDefinition && method.GetGenericArguments().Length == 2 && method.GetParameters().Length == 0);
        var commands = typeof(GrpcServer.Contracts.Commands.Project.CreateProjectCommand).Assembly.GetTypes()
            .Select(type => (Command: type, Result: type.GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(FastEndpoints.ICommand<>))?.GetGenericArguments()[0]))
            .Where(pair => pair.Result is not null);
        foreach (var (command, result) in commands)
            register.MakeGenericMethod(command, result!).Invoke(connection, null);
    }

    /// <summary>A token for the seeded admin (default) or member account.</summary>
    public static string CreateToken(bool admin = true, DateTime? expiresAt = null) =>
        CreateToken(admin ? Admin : Member, expiresAt);

    /// <summary>
    /// An access token shaped like the identity server's (RS256, <c>typ: at+jwt</c>, its key id),
    /// with knobs for the edge cases that a real sign-in can't produce.
    /// </summary>
    public static string CreateToken(TestAccount account, DateTime? expiresAt = null, string? stamp = null,
        bool? adminClaim = null, string issuer = Issuer, string audience = Audience, SecurityKey? signingKey = null,
        string algorithm = SecurityAlgorithms.RsaSha256, string tokenType = "at+jwt", string? sessionId = null)
    {
        var claims = new List<Claim>
        {
            new(CpnucleoClaimTypes.Subject, account.Id.ToString()),
            new(CpnucleoClaimTypes.SecurityStamp, stamp ?? account.Stamp),
            new(CpnucleoClaimTypes.Login, account.Login)
        };
        if (adminClaim ?? account.IsAdmin) claims.Add(new(CpnucleoClaimTypes.Admin, "true"));
        if (sessionId is not null) claims.Add(new("sid", sessionId));
        var handler = new JwtSecurityTokenHandler();
        return handler.WriteToken(handler.CreateJwtSecurityToken(new SecurityTokenDescriptor
        {
            Issuer = issuer,
            Audience = audience,
            Subject = new ClaimsIdentity(claims),
            Expires = expiresAt ?? DateTime.UtcNow.AddMinutes(10),
            NotBefore = (expiresAt ?? DateTime.UtcNow.AddMinutes(10)).AddMinutes(-30),
            TokenType = tokenType,
            SigningCredentials = new SigningCredentials(signingKey ?? new RsaSecurityKey(SigningKey) { KeyId = SigningKeyId }, algorithm)
        }));
    }

    public IServiceProvider GrpcServices => grpcFactory.Services;

    public HttpMessageHandler GrpcHandler() => grpcFactory.Server.CreateHandler();

    public HttpClient CreateClient() => factory.CreateClient();

    public HttpClient CreateClient(TestAccount account)
    {
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CreateToken(account));
        return client;
    }

    public static CallOptions GrpcOptions(bool admin = true) => GrpcOptions(admin ? Admin : Member);

    public static CallOptions GrpcOptions(TestAccount account) => GrpcOptions(CreateToken(account, audience: GrpcAudience));

    public static CallOptions GrpcOptions(string? token) => new(
        headers: token is null ? [] : new Metadata { { "Authorization", $"Bearer {token}" } },
        cancellationToken: TestContext.Current.CancellationToken);

    public async ValueTask DisposeAsync()
    {
        Client.Dispose();
        await identityFactory.DisposeAsync();
        await grpcFactory.DisposeAsync();
        await factory.DisposeAsync();
        foreach (var rootFactory in rootFactories) await rootFactory.DisposeAsync();
        await database.DisposeAsync();
    }

    private sealed class FailureLoggerProvider(System.Collections.Concurrent.ConcurrentQueue<string> messages) : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName) => new FailureLogger(messages);
        public void Dispose() { }
        private sealed class FailureLogger(System.Collections.Concurrent.ConcurrentQueue<string> messages) : ILogger
        {
            public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
            public bool IsEnabled(LogLevel level) => level >= LogLevel.Error;
            public void Log<TState>(LogLevel level, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
            {
                if (exception is not null) messages.Enqueue(exception.GetBaseException().Message);
            }
        }
    }
}

[CollectionDefinition("Database")]
public class DatabaseCollection : ICollectionFixture<WebAppFixture>;
