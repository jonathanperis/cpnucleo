namespace Infrastructure.Common.Repositories;

public class UserProjectRepository(IConfiguration configuration) : IUserProjectRepository
{
    public async Task<bool> CreateUserProject(UserProject userProject)
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           INSERT INTO "UserProject" ("Id", "UserId", "ProjectId", "CreatedAt", "Active")
                           VALUES (@Id, @UserId, @ProjectId, @CreatedAt, @Active);
                           """;

        return await connection.ExecuteAsync(sql, new { userProject.Id, userProject.UserId, userProject.ProjectId, userProject.CreatedAt, userProject.Active }) == 1;
    }

    public async Task<UserProjectDto?> GetUserProjectById(Ulid id)
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           SELECT "Id", "UserId", "ProjectId", "CreatedAt", "UpdatedAt", "Active"
                           FROM "UserProject"
                           WHERE "Id" = @Id AND "Active" = 1;
                           """;

        return await connection.QueryFirstOrDefaultAsync<UserProjectDto>(sql, new { Id = id });
    }

    public async Task<List<UserProjectDto>?> ListUserProjects()
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           SELECT "Id", "UserId", "ProjectId", "CreatedAt", "UpdatedAt", "Active"
                           FROM "UserProject"
                           WHERE "Active" = 1;
                           """;

        return (await connection.QueryAsync<UserProjectDto>(sql)).AsList();
    }

    public async Task<bool> RemoveUserProject(Ulid id)
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           UPDATE "UserProject"
                           SET "Active" = 0, "DeletedAt" = @DeletedAt
                           WHERE "Id" = @Id;
                           """;

        var deletedAt = DateTime.UtcNow;

        return await connection.ExecuteAsync(sql, new { DeletedAt = deletedAt, Id = id }) == 1;
    }

    public async Task<bool> UpdateUserProject(Ulid id, Ulid userId, Ulid projectId)
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           UPDATE "UserProject"
                           SET "UserId" = @UserId, "ProjectId" = @ProjectId, "UpdatedAt" = @UpdatedAt
                           WHERE "Id" = @Id;
                           """;

        var updatedAt = DateTime.UtcNow;

        return await connection.ExecuteAsync(sql, new { UserId = userId, ProjectId = projectId, UpdatedAt = updatedAt, Id = id }) == 1;
    }
}
