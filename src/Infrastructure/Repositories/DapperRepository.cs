namespace Infrastructure.Repositories;

public class DapperRepository<T>(NpgsqlConnection connection, NpgsqlTransaction? transaction, string tableName) : IRepository<T>
    where T : BaseEntity
{
    private const string PrimaryKey = "Id";
    
    // Cache reflection results to avoid repeated GetProperties calls
    private static readonly Lazy<PropertyInfo[]> CachedProperties = new(() => 
        typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance));
    
    private static readonly Lazy<Dictionary<string, string>> CachedPropertyNames = new(() =>
        CachedProperties.Value.Where(IsColumnProperty).ToDictionary(p => p.Name, p => p.Name, StringComparer.OrdinalIgnoreCase));

    public async Task<T?> GetByIdAsync(Guid id)
    {
        var sql = $"""
                   SELECT * FROM "{tableName}" 
                   WHERE "{PrimaryKey}" = @Id AND "Active" = true
                   """;       
        
        return await connection.QueryFirstOrDefaultAsync<T>(sql,
            new { Id = id }, transaction);
    }

    public async Task<PaginatedResult<T?>> GetAllAsync(PaginationParams pagination, CancellationToken cancellationToken = default)
    {
        var validSortColumn = ValidateSortColumn(pagination.SortColumn);
        var ids = pagination.GetIds();
        var validSortOrder = pagination.SortOrder?.ToUpper() == "DESC" ? "DESC" : "ASC";
        var searchColumns = new[] { "Name", "Description", "Login" }.Where(CachedPropertyNames.Value.ContainsKey).ToArray();
        var searchClause = pagination.Search is not null && searchColumns.Length > 0
            ? " AND (" + string.Join(" OR ", searchColumns.Select(column => $"\"{column}\" ILIKE @Search")) + ")"
            : string.Empty;

        var sql = $"""
                   SELECT * FROM "{tableName}" 
                   WHERE "Active" = true {searchClause} AND (NOT @FilterIds OR "Id" = ANY(@Ids))
                   ORDER BY "{validSortColumn}" {validSortOrder}, "Id" ASC
                   OFFSET @Offset LIMIT @PageSize;
                   
                   SELECT COUNT(*) FROM "{tableName}" WHERE "Active" = true {searchClause} AND (NOT @FilterIds OR "Id" = ANY(@Ids));
                   """;

        var command = new CommandDefinition(sql, new
        {
            pagination.Offset,
            pagination.PageSize,
            Search = $"%{pagination.Search}%",
            FilterIds = ids.Length > 0,
            Ids = ids
        }, transaction, cancellationToken: cancellationToken);

        await using var multi = await connection.QueryMultipleAsync(command);

        return new PaginatedResult<T?>
        {
            Data = await multi.ReadAsync<T>(),
            TotalCount = await multi.ReadSingleAsync<int>(),
            PageNumber = pagination.PageNumber.GetValueOrDefault(),
            PageSize = pagination.PageSize.GetValueOrDefault()
        };
    }

    public async Task<Guid> AddAsync(T? entity)
    {
        var columns = GetColumns(excludeKey: false);
        var properties = GetPropertyNames(excludeKey: false);
            
        var sql = $"""
                   INSERT INTO "{tableName}" ({columns})
                   VALUES ({properties}) RETURNING "Id"
                   """;              
        
        return await connection.ExecuteScalarAsync<Guid>(sql, entity, transaction);
    }

    public async Task<bool> UpdateAsync(T? entity)
    {
        var properties = GetUpdatePropertyNames();

        var sql = $"""
                   UPDATE "{tableName}"
                   SET {properties}
                   WHERE "{PrimaryKey}" = @Id
                   """;

        var affectedRows = await connection.ExecuteAsync(sql, entity, transaction);
        return affectedRows > 0;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var sql = $"""
                   UPDATE "{tableName}"
                   SET "Active" = false, "DeletedAt" = now()
                   WHERE "{PrimaryKey}" = @Id AND "Active" = true
                   """;        
        
        var affectedRows = await connection.ExecuteAsync(sql, new { Id = id }, transaction);
        return affectedRows > 0;
    }

    public async Task<bool> UpdateIfVersionAsync(T entity, DateTime expectedVersion, CancellationToken cancellationToken = default)
    {
        var values = new DynamicParameters(entity);
        values.Add("ExpectedVersion", expectedVersion);
        var assignments = GetUpdatePropertyNames().Replace("\"UpdatedAt\" = @UpdatedAt",
            "\"UpdatedAt\" = GREATEST(@UpdatedAt, COALESCE(\"UpdatedAt\", \"CreatedAt\") + interval '1 microsecond')", StringComparison.Ordinal);
        return await connection.ExecuteAsync(new CommandDefinition($"""
            UPDATE "{tableName}" SET {assignments}
            WHERE "Id" = @Id AND "Active" = true AND COALESCE("UpdatedAt", "CreatedAt") = @ExpectedVersion
            """, values, transaction, cancellationToken: cancellationToken)) == 1;
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        var sql = $"""
                   SELECT EXISTS(SELECT 1 FROM "{tableName}"
                   WHERE "{PrimaryKey}" = @Id AND "Active" = true)
                   """;   
        
        return await connection.ExecuteScalarAsync<bool>(sql, new { Id = id }, transaction);
    }

    private static string ValidateSortColumn(string? column)
    {
        return !string.IsNullOrWhiteSpace(column) && CachedPropertyNames.Value.TryGetValue(column, out var canonicalName)
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

    private static string GetUpdatePropertyNames()
    {
        return string.Join(", ", GetProperties(excludeKey: true)
            .Select(p => $"\"{p.Name}\" = @{p.Name}"));
    }

    private static string GetPropertyNames(bool excludeKey = false)
    {
        return string.Join(", ", GetProperties(excludeKey)
            .Select(p => $"@{p.Name}"));
    }
}
