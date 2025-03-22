namespace Infrastructure.Repositories;

//[DapperAot]
public class ProjectRepository(NpgsqlConnection connection) : IProjectRepository
{    
    public async Task<Project?> GetByIdAsync(Guid id)
    {
        return await connection.QueryFirstOrDefaultAsync<Project>(
            $"""
                SELECT * FROM "Projects" WHERE "Id" = @Id AND "Active" = true
                """,
            new { Id = id });
    }

    public async Task<PaginatedResult<Project?>> GetAllAsync(PaginationParams pagination)
    {
        var validSortColumn = ValidateSortColumn(pagination.SortColumn);
        var validSortOrder = pagination.SortOrder?.ToUpper() == "DESC" ? "DESC" : "ASC";

        var sql = $"""
                   SELECT * FROM "Projects" 
                   WHERE "Active" = true
                   ORDER BY "{validSortColumn}" {validSortOrder}
                   OFFSET @Offset LIMIT @PageSize;
                   
                   SELECT COUNT(*) FROM "Projects";
                   """;

        await using var multi = await connection.QueryMultipleAsync(sql, new
        {
            pagination.Offset,
            pagination.PageSize
        });

        return new PaginatedResult<Project?>
        {
            Data = await multi.ReadAsync<Project>(),
            TotalCount = await multi.ReadSingleAsync<int>(),
            PageNumber = pagination.PageNumber.GetValueOrDefault(),
            PageSize = pagination.PageSize.GetValueOrDefault(),
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
                             SET "Name" = @Name, "OrganizationId" = @OrganizationId, "UpdatedAt" = @UpdatedAt
                             WHERE "Id" = @Id;
                             """;

        var affectedRows = await connection.ExecuteAsync(query, entity);
        return affectedRows > 0;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        const string query = """
                             DELETE FROM "Projects" WHERE "Id" = @Id";
                             """;
        
        var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
        return affectedRows > 0;
    }
    
    private static string ValidateSortColumn(string? column)
    {
        var properties = typeof(Project).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        return properties.Any(p => p.Name.Equals(column, StringComparison.OrdinalIgnoreCase)) 
            ? column!
            : "Id";
    }
}
