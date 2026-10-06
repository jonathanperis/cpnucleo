using Dapper;
using Domain.Entities;
using Infrastructure.Common.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Npgsql;
using Testcontainers.PostgreSql;

namespace WebApi.Integration.Tests;

/// <summary>A database of its own for tests that change schemas or reset data.</summary>
public sealed class IsolatedDatabase : IAsyncLifetime
{
    private readonly PostgreSqlContainer container = new PostgreSqlBuilder("postgres:16.15").Build();

    public string ConnectionString(string database) =>
        new NpgsqlConnectionStringBuilder(container.GetConnectionString()) { Database = database }.ConnectionString;

    public async Task<string> CreateDatabaseAsync()
    {
        var name = $"isolated_{Guid.NewGuid():N}";
        await using var connection = new NpgsqlConnection(container.GetConnectionString());
        await connection.ExecuteAsync($"CREATE DATABASE {name}");
        return ConnectionString(name);
    }

    public static ApplicationDbContext Context(string connectionString) =>
        new(new DbContextOptionsBuilder<ApplicationDbContext>().UseNpgsql(connectionString).Options);

    public ValueTask InitializeAsync() => new(container.StartAsync());

    public ValueTask DisposeAsync() => container.DisposeAsync();
}

public class IsolatedDatabaseTests(IsolatedDatabase database) : IClassFixture<IsolatedDatabase>
{
    private static CancellationToken Cancellation => TestContext.Current.CancellationToken;

    [Fact]
    public async Task Readiness_RequiresTheNewestMigration()
    {
        var connectionString = await database.CreateDatabaseAsync();
        await using var dataSource = NpgsqlDataSource.Create(connectionString);
        var check = new DatabaseReadinessCheck(dataSource);
        var context = new HealthCheckContext();

        (await check.CheckHealthAsync(context, Cancellation)).Status.ShouldBe(HealthStatus.Unhealthy, "an empty database has no schema");

        await using (var db = IsolatedDatabase.Context(connectionString))
            await Microsoft.EntityFrameworkCore.Infrastructure.AccessorExtensions.GetService<Microsoft.EntityFrameworkCore.Migrations.IMigrator>(db)
                .MigrateAsync("20261006000100_RelationshipIntegrity", Cancellation);
        var stale = await check.CheckHealthAsync(context, Cancellation);
        stale.Status.ShouldBe(HealthStatus.Unhealthy, "an instance must not serve a database the migrator hasn't upgraded");
        stale.Description!.ShouldContain(DatabaseReadinessCheck.LatestMigrationId);

        await using (var db = IsolatedDatabase.Context(connectionString))
            await db.Database.MigrateAsync(Cancellation);
        (await check.CheckHealthAsync(context, Cancellation)).Status.ShouldBe(HealthStatus.Healthy);
    }

    [Fact]
    public async Task FailedExplicitSeed_RollsBackItsReset()
    {
        var connectionString = await database.CreateDatabaseAsync();
        await using (var db = IsolatedDatabase.Context(connectionString))
            await db.Database.MigrateAsync(Cancellation);
        var organization = Organization.Create("Preserved after seed failure", "");
        await using (var db = IsolatedDatabase.Context(connectionString))
        {
            db.Add(organization);
            await db.SaveChangesAsync(Cancellation);
        }

        // The importer requires CPNUCLEO_DEMO_PASSWORD; without it the run fails after its TRUNCATE.
        var previous = Environment.GetEnvironmentVariable("CPNUCLEO_DEMO_PASSWORD");
        try
        {
            Environment.SetEnvironmentVariable("CPNUCLEO_DEMO_PASSWORD", null);
            await Should.ThrowAsync<InvalidOperationException>(() => Infrastructure.Common.Helpers.FakeDataCsvImporter.RunAsync(
                connectionString, Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance, Cancellation));
        }
        finally { Environment.SetEnvironmentVariable("CPNUCLEO_DEMO_PASSWORD", previous); }

        await using var connection = new NpgsqlConnection(connectionString);
        (await connection.ExecuteScalarAsync<int>("SELECT count(*) FROM \"Organizations\" WHERE \"Id\" = @id", new { id = organization.Id })).ShouldBe(1);
        (await connection.ExecuteScalarAsync<int>("""
            SELECT count(*) FROM pg_trigger WHERE tgname LIKE '%_ActiveParents' AND tgenabled = 'D'
            """)).ShouldBe(0, "the rollback also restores the relationship triggers");
    }
}
