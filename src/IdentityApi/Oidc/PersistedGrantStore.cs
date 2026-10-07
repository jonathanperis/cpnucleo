using Dapper;
using Open.IdentityServer.Models;
using Open.IdentityServer.Stores;

namespace IdentityApi.Oidc;

/// <summary>
/// PostgreSQL store for authorization codes and refresh tokens (<c>IdentityPersistedGrants</c>).
/// Keys arrive already hashed by the server, so raw token handles never reach the database.
/// </summary>
public sealed class PersistedGrantStore(NpgsqlDataSource dataSource) : IPersistedGrantStore
{
    private const string Columns = """
        "Key", "Type", "SubjectId", "SessionId", "ClientId", "Description", "CreationTime", "Expiration", "ConsumedTime", "Data"
        """;

    public async Task StoreAsync(PersistedGrant grant)
    {
        await using var connection = await dataSource.OpenConnectionAsync();
        await connection.ExecuteAsync($"""
            INSERT INTO "IdentityPersistedGrants" ({Columns})
            VALUES (@Key, @Type, @SubjectId, @SessionId, @ClientId, @Description, @CreationTime, @Expiration, @ConsumedTime, @Data)
            ON CONFLICT ("Key") DO UPDATE SET
                "Type" = excluded."Type", "SubjectId" = excluded."SubjectId", "SessionId" = excluded."SessionId",
                "ClientId" = excluded."ClientId", "Description" = excluded."Description", "CreationTime" = excluded."CreationTime",
                "Expiration" = excluded."Expiration", "ConsumedTime" = excluded."ConsumedTime", "Data" = excluded."Data"
            """, new
        {
            grant.Key, grant.Type, grant.SubjectId, grant.SessionId, grant.ClientId, grant.Description,
            CreationTime = Utc(grant.CreationTime), Expiration = Utc(grant.Expiration), ConsumedTime = Utc(grant.ConsumedTime), grant.Data
        });
    }

    public async Task<PersistedGrant?> GetAsync(string key)
    {
        await using var connection = await dataSource.OpenConnectionAsync();
        return await connection.QuerySingleOrDefaultAsync<PersistedGrant>(
            $"""SELECT {Columns} FROM "IdentityPersistedGrants" WHERE "Key" = @key""", new { key });
    }

    public async Task<IEnumerable<PersistedGrant>> GetAllAsync(PersistedGrantFilter filter)
    {
        Open.IdentityServer.Extensions.PersistedGrantFilterExtensions.Validate(filter);
        var (where, parameters) = Where(filter);
        await using var connection = await dataSource.OpenConnectionAsync();
        return (await connection.QueryAsync<PersistedGrant>($"""SELECT {Columns} FROM "IdentityPersistedGrants" WHERE {where}""", parameters)).ToList();
    }

    public async Task RemoveAsync(string key)
    {
        await using var connection = await dataSource.OpenConnectionAsync();
        await connection.ExecuteAsync("""DELETE FROM "IdentityPersistedGrants" WHERE "Key" = @key""", new { key });
    }

    public async Task RemoveAllAsync(PersistedGrantFilter filter)
    {
        Open.IdentityServer.Extensions.PersistedGrantFilterExtensions.Validate(filter);
        var (where, parameters) = Where(filter);
        await using var connection = await dataSource.OpenConnectionAsync();
        await connection.ExecuteAsync($"""DELETE FROM "IdentityPersistedGrants" WHERE {where}""", parameters);
    }

    /// <summary>Only the filter's own columns, always parameterized.</summary>
    private static (string Where, DynamicParameters Parameters) Where(PersistedGrantFilter filter)
    {
        var clauses = new List<string>();
        var parameters = new DynamicParameters();
        void Add(string column, string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return;
            clauses.Add($"\"{column}\" = @{column}");
            parameters.Add(column, value);
        }

        Add("SubjectId", filter.SubjectId);
        Add("SessionId", filter.SessionId);
        Add("ClientId", filter.ClientId);
        Add("Type", filter.Type);
        return (string.Join(" AND ", clauses), parameters);
    }

    private static DateTime Utc(DateTime value) => DateTime.SpecifyKind(value, DateTimeKind.Utc);

    private static DateTime? Utc(DateTime? value) => value is { } date ? Utc(date) : null;
}
