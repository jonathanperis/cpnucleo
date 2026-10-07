using Dapper;

namespace IdentityApi.Oidc;

/// <summary>
/// Server-side sign-in sessions (<c>IdentitySessions</c>), keyed by the OIDC session id (<c>sid</c>)
/// that every token of the session carries. Ending a session deletes its refresh tokens here, and
/// API hosts reject its access tokens (<see cref="Infrastructure.Security.TokenSessionValidator"/>).
/// </summary>
public sealed class IdentitySessions(NpgsqlDataSource dataSource, TimeProvider timeProvider, ILogger<IdentitySessions> logger)
{
    public async Task StartAsync(string sessionId, Guid userId, CancellationToken cancellationToken = default)
    {
        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        await connection.ExecuteAsync(new CommandDefinition("""
            INSERT INTO "IdentitySessions" ("Id", "UserId", "StartedAt") VALUES (@sessionId, @userId, @now)
            ON CONFLICT ("Id") DO NOTHING
            """, new { sessionId, userId, now = timeProvider.GetUtcNow().UtcDateTime }, cancellationToken: cancellationToken));
    }

    /// <summary>Ends the session and revokes its refresh tokens and unused authorization codes.</summary>
    public async Task EndAsync(string? sessionId, string reason, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(sessionId)) return;
        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);
        var ended = await connection.ExecuteAsync(new CommandDefinition("""
            UPDATE "IdentitySessions" SET "EndedAt" = @now WHERE "Id" = @sessionId AND "EndedAt" IS NULL
            """, new { sessionId, now = timeProvider.GetUtcNow().UtcDateTime }, transaction, cancellationToken: cancellationToken));
        await connection.ExecuteAsync(new CommandDefinition("""
            DELETE FROM "IdentityPersistedGrants" WHERE "SessionId" = @sessionId
            """, new { sessionId }, transaction, cancellationToken: cancellationToken));
        await transaction.CommitAsync(cancellationToken);
        if (ended > 0) logger.LogInformation("Ended identity session ({Reason}).", reason);
    }
}
