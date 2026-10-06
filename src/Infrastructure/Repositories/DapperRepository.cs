namespace Infrastructure.Repositories;

/// <summary>
/// Generic Dapper repository ("Dapper Repository Advanced"). Reads are restricted to rows the
/// current user may see (<see cref="AccessSql"/>), writes are authorized by <see cref="IAccessGuard"/>,
/// and updates never touch identity or soft-delete columns, so an update racing a removal cannot
/// bring the removed row back.
/// </summary>
public class DapperRepository<T>(
    NpgsqlConnection connection,
    Func<NpgsqlTransaction?> transaction,
    string tableName,
    ICurrentUser currentUser,
    IAccessGuard accessGuard) : IRepository<T>
    where T : BaseEntity
{
    private const string PrimaryKey = "Id";

    // Identity and lifecycle columns are written by INSERT and soft-delete statements only.
    private static readonly HashSet<string> ImmutableColumns = new(StringComparer.OrdinalIgnoreCase)
        { "Id", "CreatedAt", "Active", "DeletedAt" };

    // Credential columns are never exposed as sort keys.
    private static readonly HashSet<string> UnsortableColumns = new(StringComparer.OrdinalIgnoreCase)
        { "Password", "Salt" };

    // Cache reflection results to avoid repeated GetProperties calls
    private static readonly Lazy<PropertyInfo[]> CachedProperties = new(() =>
        typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance));

    private static readonly Lazy<Dictionary<string, string>> CachedPropertyNames = new(() =>
        CachedProperties.Value.Where(IsColumnProperty).ToDictionary(p => p.Name, p => p.Name, StringComparer.OrdinalIgnoreCase));

    private static readonly string Visibility = AccessSql.ReadPredicate(typeof(T));

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var sql = $"""
                   SELECT * FROM "{tableName}"
                   WHERE "{PrimaryKey}" = @Id AND "Active" = true{Visibility}
                   """;

        return await connection.QueryFirstOrDefaultAsync<T>(new CommandDefinition(sql,
            WithAccess(new { Id = id }), transaction(), cancellationToken: cancellationToken));
    }

    public async Task<PaginatedResult<T?>> GetAllAsync(PaginationParams pagination, CancellationToken cancellationToken = default)
    {
        var validSortColumn = ValidateSortColumn(pagination.SortColumn);
        var ids = pagination.GetIds();
        var validSortOrder = pagination.SortOrder == "DESC" ? "DESC" : "ASC";
        var searchPattern = pagination.GetSearchPattern();
        var searchColumns = new[] { "Name", "Description", "Login" }.Where(CachedPropertyNames.Value.ContainsKey).ToArray();
        var searchClause = searchPattern is not null && searchColumns.Length > 0
            ? " AND (" + string.Join(" OR ", searchColumns.Select(column => $"\"{column}\" ILIKE @Search ESCAPE '\\'")) + ")"
            : string.Empty;
        var filter = $"""WHERE "Active" = true{searchClause} AND (NOT @FilterIds OR "Id" = ANY(@Ids)){Visibility}""";

        var sql = $"""
                   SELECT * FROM "{tableName}"
                   {filter}
                   ORDER BY "{validSortColumn}" {validSortOrder}, "Id" ASC
                   OFFSET @Offset LIMIT @PageSize;

                   SELECT COUNT(*) FROM "{tableName}" {filter};
                   """;

        var command = new CommandDefinition(sql, WithAccess(new
        {
            pagination.Offset,
            pagination.PageSize,
            Search = searchPattern,
            FilterIds = ids.Length > 0,
            Ids = ids
        }), transaction(), cancellationToken: cancellationToken);

        await using var multi = await connection.QueryMultipleAsync(command);

        return new PaginatedResult<T?>
        {
            Data = await multi.ReadAsync<T>(),
            TotalCount = await multi.ReadSingleAsync<int>(),
            PageNumber = pagination.PageNumber.GetValueOrDefault(),
            PageSize = pagination.PageSize.GetValueOrDefault()
        };
    }

    public async Task<Guid> AddAsync(T? entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        await accessGuard.EnsureCanWriteAsync(ResourceAccess.TargetOf(entity), AccessOperation.Create, Session(), cancellationToken);

        var columns = GetColumns(excludeKey: false);
        var properties = GetPropertyNames(excludeKey: false);

        var sql = $"""
                   INSERT INTO "{tableName}" ({columns})
                   VALUES ({properties}) RETURNING "Id"
                   """;

        return await connection.ExecuteScalarAsync<Guid>(new CommandDefinition(sql, entity, transaction(), cancellationToken: cancellationToken));
    }

    public async Task<bool> UpdateAsync(T? entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        if (!await AuthorizeModificationAsync(entity, cancellationToken)) return false;

        var sql = $"""
                   UPDATE "{tableName}"
                   SET {GetUpdateAssignments()}
                   WHERE "{PrimaryKey}" = @Id AND "Active" = true
                   """;

        var affectedRows = await connection.ExecuteAsync(new CommandDefinition(sql, entity, transaction(), cancellationToken: cancellationToken));
        return affectedRows > 0;
    }

    public async Task<bool> UpdateIfVersionAsync(T entity, DateTime expectedVersion, CancellationToken cancellationToken = default)
    {
        if (!await AuthorizeModificationAsync(entity, cancellationToken)) return false;

        var values = new DynamicParameters(entity);
        values.Add("ExpectedVersion", Guard.Utc(expectedVersion));
        return await connection.ExecuteAsync(new CommandDefinition($"""
            UPDATE "{tableName}" SET {GetUpdateAssignments()}
            WHERE "Id" = @Id AND "Active" = true AND COALESCE("UpdatedAt", "CreatedAt") = @ExpectedVersion
            """, values, transaction(), cancellationToken: cancellationToken)) == 1;
    }

    public async Task<bool> RemoveManyAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken = default)
    {
        var distinctIds = BatchIds.Normalize(ids);

        var ambient = transaction();
        await using var local = ambient is null ? await BeginLocalTransactionAsync(cancellationToken) : null;
        var current = ambient ?? local;

        // Lock the batch in a stable order: concurrent overlapping batches queue instead of
        // deadlocking, and nothing can move or remove these rows between the check and the write.
        var rows = (await connection.QueryAsync<T>(new CommandDefinition($"""
            SELECT * FROM "{tableName}" WHERE "Id" = ANY(@Ids) AND "Active" = true{Visibility}
            ORDER BY "Id" FOR UPDATE
            """, WithAccess(new { Ids = distinctIds }), current, cancellationToken: cancellationToken))).ToArray();
        if (rows.Length != distinctIds.Length)
        {
            if (local is not null) await local.RollbackAsync(CancellationToken.None);
            return false;
        }

        var session = new DatabaseSession(connection, current);
        foreach (var target in rows.Select(ResourceAccess.TargetOf).Distinct())
            await accessGuard.EnsureCanWriteAsync(target, AccessOperation.Modify, session, cancellationToken);

        var affected = await connection.ExecuteAsync(new CommandDefinition($"""
            UPDATE "{tableName}" SET "Active" = false, "DeletedAt" = now()
            WHERE "Id" = ANY(@Ids) AND "Active" = true
            """, new { Ids = distinctIds }, current, cancellationToken: cancellationToken));
        if (affected != distinctIds.Length)
        {
            if (local is not null) await local.RollbackAsync(CancellationToken.None);
            return false;
        }

        if (local is not null) await local.CommitAsync(cancellationToken);
        return true;
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        // Identity checks deliberately ignore visibility: an id is taken even if the caller can't see it.
        var sql = $"""
                   SELECT EXISTS(SELECT 1 FROM "{tableName}"
                   WHERE "{PrimaryKey}" = @Id AND "Active" = true)
                   """;

        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql, new { Id = id }, transaction(), cancellationToken: cancellationToken));
    }

    /// <summary>
    /// Checks the caller may modify the stored row (and move it to the new values). Returns false
    /// when the row is missing or invisible, so callers report "not found".
    /// </summary>
    private async Task<bool> AuthorizeModificationAsync(T entity, CancellationToken cancellationToken)
    {
        var original = await GetByIdAsync(entity.Id, cancellationToken);
        if (original is null) return false;

        var originalTarget = ResourceAccess.TargetOf(original);
        var newTarget = ResourceAccess.TargetOf(entity);
        await accessGuard.EnsureCanWriteAsync(originalTarget, AccessOperation.Modify, Session(), cancellationToken);
        if (newTarget != originalTarget)
            await accessGuard.EnsureCanWriteAsync(newTarget, AccessOperation.Reassign, Session(), cancellationToken);
        return true;
    }

    private DatabaseSession Session() => new(connection, transaction());

    private async Task<NpgsqlTransaction> BeginLocalTransactionAsync(CancellationToken cancellationToken)
    {
        if (connection.State != System.Data.ConnectionState.Open) await connection.OpenAsync(cancellationToken);
        return await connection.BeginTransactionAsync(cancellationToken);
    }

    private DynamicParameters WithAccess(object values)
    {
        var parameters = new DynamicParameters(values);
        parameters.AddDynamicParams(AccessSql.Parameters(currentUser));
        return parameters;
    }

    private static string ValidateSortColumn(string? column)
    {
        return !string.IsNullOrWhiteSpace(column)
            && !UnsortableColumns.Contains(column)
            && CachedPropertyNames.Value.TryGetValue(column, out var canonicalName)
            ? canonicalName
            : PrimaryKey;
    }

    private static IEnumerable<PropertyInfo> GetProperties(bool excludeKey = false)
    {
        var properties = CachedProperties.Value.Where(IsColumnProperty);

        return excludeKey
            ? properties.Where(p => !p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase))
            : properties;
    }

    private static bool IsColumnProperty(PropertyInfo property)
    {
        var type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

        return type.IsPrimitive
            || type.IsEnum
            || type == typeof(string)
            || type == typeof(Guid)
            || type == typeof(DateTime)
            || type == typeof(DateTimeOffset)
            || type == typeof(decimal);
    }

    private static string GetColumns(bool excludeKey = false)
    {
        return string.Join(", ", GetProperties(excludeKey)
            .Select(p => $"\"{p.Name}\""));
    }

    /// <summary>
    /// SET list for updates: mutable columns only. <c>UpdatedAt</c> always moves forward from the
    /// stored version, so versions stay monotonic even if application clocks disagree.
    /// </summary>
    internal static string GetUpdateAssignments()
    {
        return string.Join(", ", GetProperties(excludeKey: true)
            .Where(p => !ImmutableColumns.Contains(p.Name))
            .Select(p => p.Name == "UpdatedAt"
                ? "\"UpdatedAt\" = GREATEST(@UpdatedAt, COALESCE(\"UpdatedAt\", \"CreatedAt\") + interval '1 microsecond')"
                : $"\"{p.Name}\" = @{p.Name}"));
    }

    private static string GetPropertyNames(bool excludeKey = false)
    {
        return string.Join(", ", GetProperties(excludeKey)
            .Select(p => $"@{p.Name}"));
    }
}
