namespace Infrastructure.Repositories;

//[DapperAot]
public class ProjectRepository(IConfiguration configuration) : IProjectRepository
{
    public async Task<bool> CreateProject(Project project)
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           INSERT INTO "Projects" ("Id", "Name", "OrganizationId", "CreatedAt", "Active")
                           VALUES (@Id, @Name, @OrganizationId, @CreatedAt, @Active);
                           """;

        return await connection.ExecuteAsync(sql, new { project.Id, project.Name, project.OrganizationId, project.CreatedAt, project.Active }) == 1;
    }

    public async Task<ProjectDto?> GetProjectById(Ulid id)
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           SELECT "Id", "Name", "OrganizationId", "CreatedAt", "UpdatedAt", "Active"
                           FROM "Projects"
                           WHERE "Id" = @Id AND "Active" = true;
                           """;

        return await connection.QueryFirstOrDefaultAsync<ProjectDto>(sql, new { Id = id });
    }

    public async Task<List<ProjectDto>?> ListProjects()
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           SELECT "Id", "Name", "OrganizationId", "CreatedAt", "UpdatedAt", "Active"
                           FROM "Projects"
                           WHERE "Active" = true;
                           """;

        return (await connection.QueryAsync<ProjectDto>(sql)).AsList();
    }

    public async Task<bool> RemoveProject(Ulid id)
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           UPDATE "Projects"
                           SET "Active" = 0, "DeletedAt" = @DeletedAt
                           WHERE "Id" = @Id;
                           """;

        var deletedAt = DateTime.UtcNow;

        return await connection.ExecuteAsync(sql, new { DeletedAt = deletedAt, Id = id }) == 1;
    }

    public async Task<bool> UpdateProject(Ulid id, string name, Ulid organizationId)
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           UPDATE "Projects"
                           SET "Name" = @Name, "OrganizationId" = @OrganizationId, "UpdatedAt" = @UpdatedAt
                           WHERE "Id" = @Id;
                           """;

        var updatedAt = DateTime.UtcNow;

        return await connection.ExecuteAsync(sql, new { Name = name, OrganizationId = organizationId, UpdatedAt = updatedAt, Id = id }) == 1;
    }
}
