using Dapper;
using Open.IdentityServer.Stores;

namespace IdentityApi.Oidc;

/// <summary>
/// Hourly housekeeping: deletes expired grants and sessions that can no longer have valid tokens, and
/// touches the signing key ring so a successor key is published ahead of rotation even when idle.
/// </summary>
public sealed class IdentityMaintenance(
    NpgsqlDataSource dataSource,
    ISigningCredentialStore signingKeys,
    TimeProvider timeProvider,
    ILogger<IdentityMaintenance> logger) : BackgroundService
{
    public static readonly TimeSpan Interval = TimeSpan.FromHours(1);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(Interval, timeProvider);
        do
        {
            try
            {
                await RunOnceAsync(stoppingToken);
            }
            catch (Exception ex) when (ex is NpgsqlException or TimeoutException or InvalidOperationException && !stoppingToken.IsCancellationRequested)
            {
                // The database may not be ready yet (the migrator runs separately); try next time.
                logger.LogWarning(ex, "Identity maintenance did not complete.");
            }
        }
        while (await timer.WaitForNextTickAsync(stoppingToken));
    }

    public async Task RunOnceAsync(CancellationToken cancellationToken)
    {
        await signingKeys.GetSigningCredentialsAsync();
        var now = timeProvider.GetUtcNow().UtcDateTime;
        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        await connection.ExecuteAsync(new CommandDefinition("""
            DELETE FROM "IdentityPersistedGrants" WHERE "Expiration" < @now;
            DELETE FROM "IdentitySessions" WHERE "StartedAt" < @sessionsBefore;
            """, new { now, sessionsBefore = now - OidcSettings.MaximumSessionLength - TimeSpan.FromDays(1) }, cancellationToken: cancellationToken));
    }
}
