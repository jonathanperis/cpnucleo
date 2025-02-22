namespace Infrastructure.Repositories;

public class DapperRepository<T>(NpgsqlConnection connection, NpgsqlTransaction transaction, string tableName) : IRepository<T>
    where T : BaseEntity
{
    private const string PrimaryKey = "Id";

    public async Task<T?> GetByIdAsync(Guid id)
    {
        var sql = $"""
                   SELECT * FROM "{tableName}" 
                   WHERE "{PrimaryKey}" = @Id AND "Active" = true
                   """;       
        
        return await connection.QueryFirstOrDefaultAsync<T>(sql,
            new { Id = id }, transaction);
    }

    public async Task<PaginatedResult<T?>> GetAllAsync(PaginationParams pagination)
    {
        var validSortColumn = ValidateSortColumn(pagination.SortColumn);
        var validSortOrder = pagination.SortOrder?.ToUpper() == "DESC" ? "DESC" : "ASC";

        var sql = $"""
                   SELECT * FROM "{tableName}" 
                   WHERE "Active" = true
                   ORDER BY "{validSortColumn}" {validSortOrder}
                   OFFSET @Offset LIMIT @PageSize;
                   
                   SELECT COUNT(*) FROM "{tableName}";
                   """;

        await using var multi = await connection.QueryMultipleAsync(sql, new
        {
            pagination.Offset,
            pagination.PageSize
        }, transaction);

        return new PaginatedResult<T?>
        {
            Data = await multi.ReadAsync<T>(),
            TotalCount = await multi.ReadSingleAsync<int>(),
            PageNumber = pagination.PageNumber.GetValueOrDefault(),
            PageSize = pagination.PageSize.GetValueOrDefault(),
        };
    }

    public async Task<Guid> AddAsync(T? entity)
    {
        var columns = GetColumns(excludeKey: true);
        var properties = GetPropertyNames(excludeKey: true);
        var query = $"INSERT INTO {tableName} ({columns}) VALUES ({properties}) RETURNING Id";

        return await connection.ExecuteScalarAsync<Guid>(query, entity, transaction);
    }

    public async Task<bool> UpdateAsync(T? entity)
    {
        var columns = GetUpdateColumns();
        var query = $"UPDATE {tableName} SET {columns} WHERE {PrimaryKey} = @Id";

        var affectedRows = await connection.ExecuteAsync(query, entity, transaction);
        return affectedRows > 0;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var query = $"DELETE FROM {tableName} WHERE {PrimaryKey} = @Id";
        var affectedRows = await connection.ExecuteAsync(query, new { Id = id }, transaction);
        return affectedRows > 0;
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        var query = $"SELECT EXISTS(SELECT 1 FROM {tableName} WHERE {PrimaryKey} = @Id AND \"Active\" = true)";
        return await connection.ExecuteScalarAsync<bool>(query, new { Id = id }, transaction);
    }

    private static string ValidateSortColumn(string? column)
    {
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        return properties.Any(p => p.Name.Equals(column, StringComparison.OrdinalIgnoreCase)) 
            ? column!
            : PrimaryKey;
    }

    private static IEnumerable<PropertyInfo> GetProperties(bool excludeKey = false)
    {
        return typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => !excludeKey || !p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase));
    }

    private static string GetColumns(bool excludeKey = false)
    {
        return string.Join(", ", GetProperties(excludeKey)
            .Select(p => p.Name));
    }

    private static string GetUpdateColumns()
    {
        return string.Join(", ", GetProperties(excludeKey: true)
            .Select(p => $"{p.Name} = @{p.Name}"));
    }

    private static string GetPropertyNames(bool excludeKey = false)
    {
        return string.Join(", ", GetProperties(excludeKey)
            .Select(p => $"@{p.Name}"));
    }
}