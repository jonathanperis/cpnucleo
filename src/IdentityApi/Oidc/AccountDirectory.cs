using Dapper;

namespace IdentityApi.Oidc;

/// <summary>The account facts the OpenID Connect server needs: who is active and their security stamp.</summary>
public sealed class AccountDirectory(NpgsqlDataSource dataSource)
{
    public sealed record Account(Guid Id, string? Login, string Stamp);

    public async Task<Account?> FindActiveAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        var row = await connection.QuerySingleOrDefaultAsync<(Guid Id, string? Login, string? Password)?>(new CommandDefinition(
            """SELECT "Id", "Login", "Password" FROM "Users" WHERE "Id" = @id AND "Active" """,
            new { id }, cancellationToken: cancellationToken));
        return row is { } value ? new Account(value.Id, value.Login, SecurityStamp.Compute(value.Password, value.Login)) : null;
    }

    /// <summary>The single active account with this login; ambiguous legacy logins match nobody.</summary>
    public async Task<Account?> FindActiveByLoginAsync(string login, CancellationToken cancellationToken = default)
    {
        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        var rows = (await connection.QueryAsync<(Guid Id, string? Login, string? Password)>(new CommandDefinition(
            """SELECT "Id", "Login", "Password" FROM "Users" WHERE "Active" AND lower(btrim("Login")) = @login LIMIT 2""",
            new { login = login.Trim().ToLowerInvariant() }, cancellationToken: cancellationToken))).ToList();
        return rows is [var only] ? new Account(only.Id, only.Login, SecurityStamp.Compute(only.Password, only.Login)) : null;
    }

    /// <summary>Deterministic tenant hint: the organization of the user's earliest active project membership.</summary>
    public async Task<Guid?> FindTenantAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        return await connection.QueryFirstOrDefaultAsync<Guid?>(new CommandDefinition("""
            SELECT p."OrganizationId" FROM "UserProjects" up
            JOIN "Projects" p ON p."Id" = up."ProjectId"
            WHERE up."UserId" = @userId AND up."Active" AND p."Active"
            ORDER BY up."CreatedAt", up."Id"
            LIMIT 1
            """, new { userId }, cancellationToken: cancellationToken));
    }
}
