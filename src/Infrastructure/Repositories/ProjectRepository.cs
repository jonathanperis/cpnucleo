namespace Infrastructure.Repositories;

//[DapperAot]
public class ProjectRepository(NpgsqlConnection connection) : IProjectRepository
{
    // Cache reflection results to avoid repeated GetProperties calls
    private static readonly Lazy<Dictionary<string, string>> CachedPropertyNames = new(() =>
    {
        var properties = typeof(Project).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        return properties.Where(p => p.PropertyType == typeof(string) || p.PropertyType.IsValueType)
            .ToDictionary(p => p.Name, p => p.Name, StringComparer.OrdinalIgnoreCase);
    });
    
    public async Task<Project?> GetByIdAsync(Guid id)
    {
        return await connection.QueryFirstOrDefaultAsync<Project>(
            $"""
                SELECT * FROM "Projects" WHERE "Id" = @Id AND "Active" = true
                """,
            new { Id = id });
    }

    public async Task<PaginatedResult<Project?>> GetAllAsync(PaginationParams pagination, CancellationToken cancellationToken = default)
    {
        var validSortColumn = ValidateSortColumn(pagination.SortColumn);
        var ids = pagination.GetIds();
        var validSortOrder = pagination.SortOrder?.ToUpper() == "DESC" ? "DESC" : "ASC";

        var sql = $"""
                   SELECT * FROM "Projects" 
                   WHERE "Active" = true AND (@Search IS NULL OR "Name" ILIKE @Search) AND (NOT @FilterIds OR "Id" = ANY(@Ids))
                   ORDER BY "{validSortColumn}" {validSortOrder}, "Id" ASC
                   OFFSET @Offset LIMIT @PageSize;
                   
                   SELECT COUNT(*) FROM "Projects" WHERE "Active" = true AND (@Search IS NULL OR "Name" ILIKE @Search) AND (NOT @FilterIds OR "Id" = ANY(@Ids));
                   """;

        var command = new CommandDefinition(sql, new
        {
            pagination.Offset,
            pagination.PageSize,
            Search = pagination.Search is null ? null : $"%{pagination.Search}%",
            FilterIds = ids.Length > 0,
            Ids = ids
        }, cancellationToken: cancellationToken);

        await using var multi = await connection.QueryMultipleAsync(command);

        return new PaginatedResult<Project?>
        {
            Data = await multi.ReadAsync<Project>(),
            TotalCount = await multi.ReadSingleAsync<int>(),
            PageNumber = pagination.PageNumber.GetValueOrDefault(),
            PageSize = pagination.PageSize.GetValueOrDefault()
        };
    }

    public async Task<Guid> AddAsync(Project? entity)
    {
        const string query = """
                             INSERT INTO "Projects" ("Id", "Name", "OrganizationId", "CreatedAt", "Active")
                             VALUES (@Id, @Name, @OrganizationId, @CreatedAt, @Active) RETURNING "Id";
                             """;

        return await connection.ExecuteScalarAsync<Guid>(query, entity);
    }

    public async Task<bool> UpdateAsync(Project? entity)
    {
        const string query = """
                             UPDATE "Projects"
                             SET "Name" = @Name,
                                 "OrganizationId" = @OrganizationId,
                                 "UpdatedAt" = @UpdatedAt,
                                 "DeletedAt" = @DeletedAt,
                                 "Active" = @Active
                             WHERE "Id" = @Id;
                             """;

        var affectedRows = await connection.ExecuteAsync(query, entity).ConfigureAwait(false);
        return affectedRows > 0;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        const string query = """
                             UPDATE "Projects" SET "Active" = false, "DeletedAt" = now()
                             WHERE "Id" = @Id AND "Active" = true;
                             """;
        
        var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
        return affectedRows > 0;
    }

    public Task<bool> UpdateIfVersionAsync(Project entity, DateTime expectedVersion, CancellationToken cancellationToken = default) =>
        new DapperRepository<Project>(connection, null, "Projects").UpdateIfVersionAsync(entity, expectedVersion, cancellationToken);

    public async Task<bool> RemoveManyAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
    {
        var distinctIds = ids.Distinct().ToArray();
        if (distinctIds.Length == 0) return false;

        await connection.OpenAsync(cancellationToken);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);
        var affected = await connection.ExecuteAsync(new CommandDefinition("""
            UPDATE "Projects" SET "Active" = false, "DeletedAt" = now()
            WHERE "Id" = ANY(@Ids) AND "Active" = true
            """, new { Ids = distinctIds }, transaction, cancellationToken: cancellationToken));
        if (affected != distinctIds.Length)
        {
            await transaction.RollbackAsync(CancellationToken.None);
            return false;
        }

        await transaction.CommitAsync(cancellationToken);
        return true;
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        var sql = $"""
                   SELECT EXISTS(SELECT 1 FROM "Projects"
                   WHERE "Id" = @Id AND "Active" = true)
                   """;
        
        return await connection.ExecuteScalarAsync<bool>(sql, new { Id = id });
    }

    private static string ValidateSortColumn(string? column)
    {
        return !string.IsNullOrWhiteSpace(column) && CachedPropertyNames.Value.TryGetValue(column, out var canonicalName)
            ? canonicalName
            : "Id";
    }
}
