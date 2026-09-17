extern alias WebApiHost;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;
using Domain.Common.Security;
using Grpc.Core;
using GrpcServer.Contracts.Commands.Project;
using GrpcServer.Contracts.Commands.User;
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
using Testcontainers.PostgreSql;

namespace WebApi.Integration.Tests.Hosts;

public sealed class WebAppFixture : IAsyncLifetime
{
    private const string SigningKey = "disposable-integration-signing-key-at-least-32-characters";
    private readonly PostgreSqlContainer database = new PostgreSqlBuilder("postgres:16.15")
        .WithCommand("-c", "track_commit_timestamp=on").Build();
    private WebApplicationFactory<WebApiHost::Program> factory = null!;
    private WebApplicationFactory<GrpcServer.Handlers.Project.CreateProjectHandler> grpcFactory = null!;
    public HttpClient Client { get; private set; } = null!;
    private readonly System.Collections.Concurrent.ConcurrentQueue<string> failures = new();
    public string FailureDetails => string.Join("\n", failures);
    public string ConnectionString => database.GetConnectionString();

    public async ValueTask InitializeAsync()
    {
        await database.StartAsync();
        await using var context = CreateDbContext();
        await context.Database.MigrateAsync();
        factory = new WebApplicationFactory<WebApiHost::Program>().WithWebHostBuilder(builder =>
        {
            ConfigureApp(builder);
            builder.ConfigureServices(ConfigureServices);
        });
        Client = factory.CreateClient();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CreateToken());
        grpcFactory = new WebApplicationFactory<GrpcServer.Handlers.Project.CreateProjectHandler>().WithWebHostBuilder(builder =>
        {
            ConfigureApp(builder);
            builder.ConfigureServices(ConfigureServices);
        });
        grpcFactory.Services.MapRemoteCore("http://localhost", connection =>
        {
            connection.ChannelOptions.HttpHandler = grpcFactory.Server.CreateHandler();
            connection.Register<GetProjectByIdCommand, GetProjectByIdResult>();
            connection.Register<RemoveProjectCommand, RemoveProjectResult>();
            connection.Register<ListUsersCommand, ListUsersResult>();
            connection.Register<GrpcServer.Contracts.Commands.Assignment.CreateAssignmentCommand, GrpcServer.Contracts.Commands.Assignment.CreateAssignmentResult>();
        });
    }

    public ApplicationDbContext CreateDbContext() => new(new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseNpgsql(ConnectionString).Options);

    private void ConfigureApp(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureAppConfiguration((_, config) => config.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["DB_CONNECTION_STRING"] = ConnectionString,
            ["Jwt:SigningKey"] = SigningKey,
            ["Jwt:Issuer"] = "cpnucleo-integration",
            ["Jwt:Audience"] = "cpnucleo-integration"
        }));
    }

    private void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging(logging => logging.AddProvider(new FailureLoggerProvider(failures))
            .AddFilter<FailureLoggerProvider>((_, level) => level >= LogLevel.Error));
        // CRUD tests exercise auth and real persistence, independently of quota tests.
        services.PostConfigure<RateLimiterOptions>(options => options.GlobalLimiter =
            PartitionedRateLimiter.Create<HttpContext, string>(_ => RateLimitPartition.GetNoLimiter("integration")));
    }

    public static string CreateToken(bool admin = true, DateTime? expiresAt = null)
    {
        var claims = new List<Claim> { new(CpnucleoClaimTypes.Subject, Guid.NewGuid().ToString()) };
        if (admin) claims.Add(new(CpnucleoClaimTypes.Admin, "true"));
        return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
            "cpnucleo-integration", "cpnucleo-integration", claims, expires: expiresAt ?? DateTime.UtcNow.AddMinutes(10),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SigningKey)), SecurityAlgorithms.HmacSha256)));
    }

    public HttpClient CreateClient() => factory.CreateClient();

    public static CallOptions GrpcOptions(bool admin = true) => new(
        headers: new Metadata { { "Authorization", $"Bearer {CreateToken(admin)}" } },
        cancellationToken: TestContext.Current.CancellationToken);

    public async ValueTask DisposeAsync()
    {
        Client.Dispose();
        await grpcFactory.DisposeAsync();
        await factory.DisposeAsync();
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
