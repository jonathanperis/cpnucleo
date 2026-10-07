extern alias WebApiHost;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;
using Dapper;
using Domain.Common.Security;
using Domain.Entities;
using Grpc.Core;
using Infrastructure.Common.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using Testcontainers.PostgreSql;

namespace WebApi.Integration.Tests.Hosts;

/// <summary>
/// Real WebApi, GrpcServer and IdentityApi hosts over one disposable PostgreSQL container with all
/// migrations applied. Seeded accounts cover the three access levels: administrator, project
/// member and outsider (an active user with no memberships).
/// </summary>
public sealed class WebAppFixture : IAsyncLifetime
{
    public const string SigningKey = "disposable-integration-signing-key-at-least-32-characters";
    public const string Issuer = "cpnucleo-integration";
    public const string Audience = "cpnucleo-integration";

    public static readonly TestAccount Admin = TestAccount.Create("admin", isAdmin: true);
    public static readonly TestAccount Member = TestAccount.Create("member");
    public static readonly TestAccount Outsider = TestAccount.Create("outsider");

    private readonly PostgreSqlContainer database = new PostgreSqlBuilder("postgres:16.15")
        .WithCommand("-c", "track_commit_timestamp=on").Build();
    private WebApplicationFactory<WebApiHost::Program> factory = null!;
    private WebApplicationFactory<GrpcServer.Handlers.Project.CreateProjectHandler> grpcFactory = null!;
    private WebApplicationFactory<IdentityApi.Security.TokenIssuer> identityFactory = null!;
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

        factory = Track(new WebApplicationFactory<WebApiHost::Program>()).WithWebHostBuilder(builder =>
        {
            ConfigureApp(builder);
            builder.ConfigureServices(DisableRateLimiting);
        });
        Client = factory.CreateClient();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CreateToken());

        grpcFactory = Track(new WebApplicationFactory<GrpcServer.Handlers.Project.CreateProjectHandler>()).WithWebHostBuilder(builder =>
        {
            ConfigureApp(builder);
            builder.ConfigureServices(DisableRateLimiting);
        });
        grpcFactory.Services.MapRemoteCore("http://localhost", connection =>
        {
            connection.ChannelOptions.HttpHandler = grpcFactory.Server.CreateHandler();
            RegisterAllCommands(connection);
        });

        identityFactory = CreateIdentityFactory(disableRateLimiting: true);
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

    /// <summary>A WebApi host with production rate limiting, for quota tests.</summary>
    public WebApplicationFactory<WebApiHost::Program> CreateRateLimitedWebApiFactory() =>
        Track(new WebApplicationFactory<WebApiHost::Program>()).WithWebHostBuilder(builder =>
        {
            ConfigureApp(builder);
            builder.ConfigureServices(CaptureFailures);
        });

    /// <summary>An IdentityApi host on the same database. Tests that exercise quotas get their own instance.</summary>
    public WebApplicationFactory<IdentityApi.Security.TokenIssuer> CreateIdentityFactory(bool disableRateLimiting) =>
        Track(new WebApplicationFactory<IdentityApi.Security.TokenIssuer>()).WithWebHostBuilder(builder =>
        {
            ConfigureApp(builder);
            if (disableRateLimiting) builder.ConfigureServices(DisableRateLimiting);
            else builder.ConfigureServices(CaptureFailures);
        });

    public HttpClient CreateIdentityClient() => identityFactory.CreateClient();

    private void ConfigureApp(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureAppConfiguration((_, config) => config.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["DB_CONNECTION_STRING"] = ConnectionString,
            ["Jwt:SigningKey"] = SigningKey,
            ["Jwt:Issuer"] = Issuer,
            ["Jwt:Audience"] = Audience,
            ["CPNUCLEO_ADMIN_LOGINS"] = Admin.Login,
            // Short window so revocation tests don't wait the production 30 seconds.
            ["Auth:SessionValidationCacheSeconds"] = "1"
        }));
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

    public static string CreateToken(TestAccount account, DateTime? expiresAt = null, string? stamp = null,
        bool? adminClaim = null, string issuer = Issuer, string audience = Audience, string signingKey = SigningKey)
    {
        var claims = new List<Claim>
        {
            new(CpnucleoClaimTypes.Subject, account.Id.ToString()),
            new(CpnucleoClaimTypes.SecurityStamp, stamp ?? account.Stamp),
            new(CpnucleoClaimTypes.Login, account.Login)
        };
        if (adminClaim ?? account.IsAdmin) claims.Add(new(CpnucleoClaimTypes.Admin, "true"));
        return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
            issuer, audience, claims, expires: expiresAt ?? DateTime.UtcNow.AddMinutes(10),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)), SecurityAlgorithms.HmacSha256)));
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

    public static CallOptions GrpcOptions(TestAccount account) => GrpcOptions(CreateToken(account));

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
