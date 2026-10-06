using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Infrastructure.Common.Context;

/// <summary>
/// Readiness: PostgreSQL answers and the schema includes the newest migration this build ships,
/// so an instance never reports ready against a database the migrator hasn't upgraded yet.
/// </summary>
public sealed class DatabaseReadinessCheck(NpgsqlDataSource dataSource) : IHealthCheck
{
    public static readonly string LatestMigrationId = typeof(ApplicationDbContext).Assembly.GetTypes()
        .Select(type => type.GetCustomAttribute<MigrationAttribute>()?.Id)
        .OfType<string>()
        .Max(StringComparer.Ordinal) ?? throw new InvalidOperationException("No EF Core migrations were found.");

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
            await using var command = new NpgsqlCommand(
                """SELECT EXISTS (SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = @id)""", connection)
            {
                CommandTimeout = 3
            };
            command.Parameters.AddWithValue("id", LatestMigrationId);
            var upToDate = (bool)(await command.ExecuteScalarAsync(cancellationToken) ?? false);
            return upToDate
                ? HealthCheckResult.Healthy()
                : HealthCheckResult.Unhealthy($"Database schema is missing migration {LatestMigrationId}.");
        }
        catch (Exception ex) when (ex is NpgsqlException or TimeoutException or InvalidOperationException)
        {
            return HealthCheckResult.Unhealthy("Database or schema is unavailable.");
        }
    }
}
