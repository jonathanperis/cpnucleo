using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Infrastructure.Common.Context;

public sealed class DatabaseReadinessCheck(IConfiguration configuration) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var connection = new NpgsqlConnection(configuration["DB_CONNECTION_STRING"]);
            await connection.OpenAsync(cancellationToken);
            await using var command = new NpgsqlCommand("SELECT 1 FROM \"Users\" LIMIT 1", connection) { CommandTimeout = 3 };
            await command.ExecuteScalarAsync(cancellationToken);
            return HealthCheckResult.Healthy();
        }
        catch (Exception ex) when (ex is NpgsqlException or TimeoutException or InvalidOperationException)
        {
            return HealthCheckResult.Unhealthy("Database or schema is unavailable.");
        }
    }
}
