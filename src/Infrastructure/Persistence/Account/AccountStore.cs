namespace Infrastructure.Persistence.Account;

/// <summary>
/// The signed-in user's own account, for the self-service <c>/me</c> endpoints. User rows are
/// otherwise administrator-only (<see cref="ResourceAccessKind.UserAdministration"/>), so these are
/// trusted statements scoped to the caller's own id (the validated token's <c>sub</c>). They only
/// change the display name or the password hash; login and lifecycle columns are never written.
/// </summary>
public sealed class AccountStore(NpgsqlDataSource dataSource, ICurrentUser currentUser)
{
    // Versions only move forward, as in the repositories.
    private const string NextVersion = """GREATEST(@UpdatedAt, COALESCE("UpdatedAt", "CreatedAt") + interval '1 microsecond')""";

    /// <summary>The caller's active account, or null without an authenticated caller or active account.</summary>
    public async Task<User?> FindCurrentAsync(CancellationToken cancellationToken = default)
    {
        if (currentUser.UserId is not { } id) return null;
        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
        return await connection.QuerySingleOrDefaultAsync<User>(new CommandDefinition(
            """SELECT * FROM "Users" WHERE "Id" = @id AND "Active" """,
            new { id }, cancellationToken: cancellationToken)).ConfigureAwait(false);
    }

    /// <summary>Saves the display name. False when the account is no longer active.</summary>
    public async Task<bool> SaveNameAsync(User user, CancellationToken cancellationToken = default)
    {
        EnsureCurrent(user);
        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
        return await connection.ExecuteAsync(new CommandDefinition($"""
            UPDATE "Users" SET "Name" = @Name, "UpdatedAt" = {NextVersion}
            WHERE "Id" = @Id AND "Active"
            """, new { user.Id, user.Name, user.UpdatedAt }, cancellationToken: cancellationToken)).ConfigureAwait(false) == 1;
    }

    /// <summary>
    /// Saves a new password hash, only while the stored hash is still <paramref name="verifiedHash"/>
    /// (the one the current password was checked against). False when the account is no longer
    /// active or its password changed concurrently.
    /// </summary>
    public async Task<bool> SavePasswordAsync(User user, string verifiedHash, CancellationToken cancellationToken = default)
    {
        EnsureCurrent(user);
        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
        return await connection.ExecuteAsync(new CommandDefinition($"""
            UPDATE "Users" SET "Password" = @Password, "Salt" = @Salt, "UpdatedAt" = {NextVersion}
            WHERE "Id" = @Id AND "Active" AND "Password" = @VerifiedHash
            """, new { user.Id, user.Password, user.Salt, user.UpdatedAt, VerifiedHash = verifiedHash },
            cancellationToken: cancellationToken)).ConfigureAwait(false) == 1;
    }

    private void EnsureCurrent(User user)
    {
        if (currentUser.UserId is not { } id || user.Id != id)
            throw new InvalidOperationException("The account store only changes the caller's own account.");
    }
}
