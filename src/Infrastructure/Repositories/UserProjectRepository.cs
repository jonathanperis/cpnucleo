namespace Infrastructure.Repositories;

public class UserProjectRepository(NpgsqlConnection connection) : IUserProjectRepository
{
    public async Task<bool> CreateUserProject(UserProject userProject)
    {
        const string sql = """
                           INSERT INTO "UserProjects" ("Id", "UserId", "ProjectId", "CreatedAt", "Active")
                           VALUES (@Id, @UserId, @ProjectId, @CreatedAt, @Active);
                           """;

        return await connection.ExecuteAsync(sql, new { userProject.Id, userProject.UserId, userProject.ProjectId, userProject.CreatedAt, userProject.Active }) == 1;
    }

    public async Task<UserProject?> GetUserProjectById(Guid id)
    {
        const string sql = """
                           SELECT "Id", "UserId", "ProjectId", "CreatedAt", "UpdatedAt", "Active"
                           FROM "UserProjects"
                           WHERE "Id" = @Id AND "Active" = true;
                           """;

        return await connection.QueryFirstOrDefaultAsync<UserProject>(sql, new { Id = id });
    }

    public async Task<List<UserProject?>?> ListUserProjects()
    {
        const string sql = """
                           SELECT "Id", "UserId", "ProjectId", "CreatedAt", "UpdatedAt", "Active"
                           FROM "UserProjects"
                           WHERE "Active" = true;
                           """;

        return (await connection.QueryAsync<UserProject?>(sql)).AsList();
    }

    public async Task<bool> RemoveUserProject(Guid id)
    {
        const string sql = """
                           UPDATE "UserProjects"
                           SET "Active" = 0, "DeletedAt" = @DeletedAt
                           WHERE "Id" = @Id;
                           """;

        var deletedAt = DateTime.UtcNow;

        return await connection.ExecuteAsync(sql, new { DeletedAt = deletedAt, Id = id }) == 1;
    }

    public async Task<bool> UpdateUserProject(Guid id, Guid userId, Guid projectId)
    {
        const string sql = """
                           UPDATE "UserProjects"
                           SET "UserId" = @UserId, "ProjectId" = @ProjectId, "UpdatedAt" = @UpdatedAt
                           WHERE "Id" = @Id;
                           """;

        var updatedAt = DateTime.UtcNow;

        return await connection.ExecuteAsync(sql, new { UserId = userId, ProjectId = projectId, UpdatedAt = updatedAt, Id = id }) == 1;
    }
}
