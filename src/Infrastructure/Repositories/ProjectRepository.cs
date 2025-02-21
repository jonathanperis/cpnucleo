namespace Infrastructure.Repositories;

//[DapperAot]
public class ProjectRepository(NpgsqlConnection connection) : IProjectRepository
{    
    private const string PrimaryKey = "Id";

    public async Task<bool> CreateProject(Project project)
    {
        const string sql = """
                           INSERT INTO "Projects" ("Id", "Name", "OrganizationId", "CreatedAt", "Active")
                           VALUES (@Id, @Name, @OrganizationId, @CreatedAt, @Active);
                           """;

        return await connection.ExecuteAsync(sql, new { project.Id, project.Name, project.OrganizationId, project.CreatedAt, project.Active }) == 1;
    }

    public async Task<Project?> GetProjectById(Guid id)
    {
        const string sql = """
                           SELECT "Id", "Name", "OrganizationId", "CreatedAt", "UpdatedAt", "Active"
                           FROM "Projects"
                           WHERE "Id" = @Id AND "Active" = true;
                           """;

        return await connection.QueryFirstOrDefaultAsync<Project>(sql, new { Id = id });
    }

    public async Task<PaginatedResult<Project?>> ListProjects(PaginationParams pagination)
    {
        var validSortColumn = ValidateSortColumn(pagination.SortColumn);
        var validSortOrder = pagination.SortOrder?.ToUpper() == "DESC" ? "DESC" : "ASC";

        var sql = $"""
                   SELECT "Id", "Name", "OrganizationId", "CreatedAt", "UpdatedAt", "Active"
                   FROM "Projects" 
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

    public async Task<bool> RemoveProject(Guid id)
    {
        const string sql = """
                           UPDATE "Projects"
                           SET "Active" = 0, "DeletedAt" = @DeletedAt
                           WHERE "Id" = @Id;
                           """;

        var deletedAt = DateTime.UtcNow;

        return await connection.ExecuteAsync(sql, new { DeletedAt = deletedAt, Id = id }) == 1;
    }

    public async Task<bool> UpdateProject(Guid id, string name, Guid organizationId)
    {
        const string sql = """
                           UPDATE "Projects"
                           SET "Name" = @Name, "OrganizationId" = @OrganizationId, "UpdatedAt" = @UpdatedAt
                           WHERE "Id" = @Id;
                           """;

        var updatedAt = DateTime.UtcNow;

        return await connection.ExecuteAsync(sql, new { Name = name, OrganizationId = organizationId, UpdatedAt = updatedAt, Id = id }) == 1;
    }
    
    private static string ValidateSortColumn(string? column)
    {
        var properties = typeof(Project).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        return properties.Any(p => p.Name.Equals(column, StringComparison.OrdinalIgnoreCase)) 
            ? column!
            : PrimaryKey;
    }
}
