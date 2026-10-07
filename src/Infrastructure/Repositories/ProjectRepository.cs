namespace Infrastructure.Repositories;

/// <summary>
/// Hand-written Dapper repository ("Dapper Repository Basic") for projects. It applies the same
/// visibility and write rules as <see cref="DapperRepository{T}"/> with explicit SQL.
/// </summary>
public class ProjectRepository(NpgsqlConnection connection, ICurrentUser currentUser, IAccessGuard accessGuard) : IProjectRepository
{
    private const string Visibility = """
        AND (@AccessAll OR "Id" IN (SELECT up."ProjectId" FROM "UserProjects" up
                                    WHERE up."UserId" = @AccessUserId AND up."Active"))
        """;

    // Cache reflection results to avoid repeated GetProperties calls
    private static readonly Lazy<Dictionary<string, string>> CachedPropertyNames = new(() =>
    {
        var properties = typeof(Project).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        return properties.Where(p => p.PropertyType == typeof(string) || p.PropertyType.IsValueType)
            .ToDictionary(p => p.Name, p => p.Name, StringComparer.OrdinalIgnoreCase);
    });

    public async Task<Project?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await connection.QueryFirstOrDefaultAsync<Project>(new CommandDefinition($"""
            SELECT * FROM "Projects" WHERE "Id" = @Id AND "Active" = true {Visibility}
            """, WithAccess(new { Id = id }), cancellationToken: cancellationToken));
    }

    public async Task<PaginatedResult<Project?>> GetAllAsync(PaginationParams pagination, CancellationToken cancellationToken = default)
    {
        // Both transports reach here; invalid or missing paging, or a filter projects can't apply
        // (only organizationId is supported), is a domain rule violation (400 / InvalidArgument).
        pagination = PaginationParams.Require(pagination, typeof(Project));
        var validSortColumn = ValidateSortColumn(pagination.SortColumn);
        var ids = pagination.GetIds();
        var validSortOrder = pagination.SortOrder == "DESC" ? "DESC" : "ASC";
        var organizationId = pagination.GetRelationFilters()
            .Where(filter => filter.Column == nameof(Project.OrganizationId))
            .Select(filter => (Guid?)filter.Value).SingleOrDefault();
        var organizationClause = organizationId is null ? string.Empty : """ AND "OrganizationId" = @OrganizationId""";
        var filter = $"""
            WHERE "Active" = true AND (@Search IS NULL OR "Name" ILIKE @Search ESCAPE '\')
              AND (NOT @FilterIds OR "Id" = ANY(@Ids)){organizationClause} {Visibility}
            """;

        var sql = $"""
                   SELECT * FROM "Projects" {filter}
                   ORDER BY "{validSortColumn}" {validSortOrder}, "Id" ASC
                   OFFSET @Offset LIMIT @PageSize;

                   SELECT COUNT(*) FROM "Projects" {filter};
                   """;

        var command = new CommandDefinition(sql, WithAccess(new
        {
            pagination.Offset,
            pagination.PageSize,
            Search = pagination.GetSearchPattern(),
            FilterIds = ids.Length > 0,
            Ids = ids,
            OrganizationId = organizationId
        }), cancellationToken: cancellationToken);

        await using var multi = await connection.QueryMultipleAsync(command);

        return new PaginatedResult<Project?>
        {
            Data = await multi.ReadAsync<Project>(),
            TotalCount = await multi.ReadSingleAsync<int>(),
            PageNumber = pagination.PageNumber.GetValueOrDefault(),
            PageSize = pagination.PageSize.GetValueOrDefault()
        };
    }

    public async Task<Guid> AddAsync(Project? entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        await accessGuard.EnsureCanWriteAsync(ResourceAccess.TargetOf(entity), AccessOperation.Create, new DatabaseSession(connection, null), cancellationToken);

        const string query = """
                             INSERT INTO "Projects" ("Id", "Name", "OrganizationId", "CreatedAt", "Active")
                             VALUES (@Id, @Name, @OrganizationId, @CreatedAt, @Active) RETURNING "Id";
                             """;

        return await connection.ExecuteScalarAsync<Guid>(new CommandDefinition(query, entity, cancellationToken: cancellationToken));
    }

    public async Task<bool> UpdateAsync(Project? entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        if (!await AuthorizeModificationAsync(entity.Id, cancellationToken)) return false;

        // Lifecycle columns are not written here, so an update can't undo a concurrent removal.
        const string query = """
                             UPDATE "Projects"
                             SET "Name" = @Name,
                                 "OrganizationId" = @OrganizationId,
                                 "UpdatedAt" = GREATEST(@UpdatedAt, COALESCE("UpdatedAt", "CreatedAt") + interval '1 microsecond')
                             WHERE "Id" = @Id AND "Active" = true;
                             """;

        var affectedRows = await connection.ExecuteAsync(new CommandDefinition(query, entity, cancellationToken: cancellationToken)).ConfigureAwait(false);
        return affectedRows > 0;
    }

    public Task<bool> UpdateIfVersionAsync(Project entity, DateTime expectedVersion, CancellationToken cancellationToken = default) =>
        CreateGenericRepository().UpdateIfVersionAsync(entity, expectedVersion, cancellationToken);

    public Task<bool> RemoveManyAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default) =>
        CreateGenericRepository().RemoveManyAsync(ids.ToArray(), cancellationToken);

    public Task<bool> RestoreManyAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default) =>
        CreateGenericRepository().RestoreManyAsync(ids.ToArray(), cancellationToken);

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        const string sql = """
                           SELECT EXISTS(SELECT 1 FROM "Projects"
                           WHERE "Id" = @Id AND "Active" = true)
                           """;

        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));
    }

    private DapperRepository<Project> CreateGenericRepository() =>
        new(connection, () => null, "Projects", currentUser, accessGuard);

    private async Task<bool> AuthorizeModificationAsync(Guid id, CancellationToken cancellationToken)
    {
        if (await GetByIdAsync(id, cancellationToken) is null) return false;
        await accessGuard.EnsureCanWriteAsync(new AccessTarget(typeof(Project), ProjectId: id), AccessOperation.Modify,
            new DatabaseSession(connection, null), cancellationToken);
        return true;
    }

    private DynamicParameters WithAccess(object values)
    {
        var parameters = new DynamicParameters(values);
        parameters.AddDynamicParams(AccessSql.Parameters(currentUser));
        return parameters;
    }

    private static string ValidateSortColumn(string? column)
    {
        return !string.IsNullOrWhiteSpace(column) && CachedPropertyNames.Value.TryGetValue(column, out var canonicalName)
            ? canonicalName
            : "Id";
    }
}
