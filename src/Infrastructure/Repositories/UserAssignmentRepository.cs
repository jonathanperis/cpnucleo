namespace Infrastructure.Repositories;

public class UserAssignmentRepository(IConfiguration configuration) : IUserAssignmentRepository
{
    public async Task<bool> CreateUserAssignment(UserAssignment userAssignment)
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           INSERT INTO "UserAssignments" ("Id", "UserId", "AssignmentId", "CreatedAt", "Active")
                           VALUES (@Id, @UserId, @AssignmentId, @CreatedAt, @Active);
                           """;

        return await connection.ExecuteAsync(sql, new { userAssignment.Id, userAssignment.UserId, userAssignment.AssignmentId, userAssignment.CreatedAt, userAssignment.Active }) == 1;
    }

    public async Task<UserAssignment?> GetUserAssignmentById(Guid id)
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           SELECT "Id", "UserId", "AssignmentId", "CreatedAt", "UpdatedAt", "Active"
                           FROM "UserAssignments"
                           WHERE "Id" = @Id AND "Active" = true;
                           """;

        return await connection.QueryFirstOrDefaultAsync<UserAssignment>(sql, new { Id = id });
    }

    public async Task<List<UserAssignment>?> ListUserAssignments()
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           SELECT "Id", "UserId", "AssignmentId", "CreatedAt", "UpdatedAt", "Active"
                           FROM "UserAssignments"
                           WHERE "Active" = true;
                           """;

        return (await connection.QueryAsync<UserAssignment>(sql)).AsList();
    }

    public async Task<bool> RemoveUserAssignment(Guid id)
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           UPDATE "UserAssignments"
                           SET "Active" = 0, "DeletedAt" = @DeletedAt
                           WHERE "Id" = @Id;
                           """;

        var deletedAt = DateTime.UtcNow;

        return await connection.ExecuteAsync(sql, new { DeletedAt = deletedAt, Id = id }) == 1;
    }

    public async Task<bool> UpdateUserAssignment(Guid id, Guid userId, Guid assignmentId)
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           UPDATE "UserAssignments"
                           SET "UserId" = @UserId, "AssignmentId" = @AssignmentId, "UpdatedAt" = @UpdatedAt
                           WHERE "Id" = @Id;
                           """;

        var updatedAt = DateTime.UtcNow;

        return await connection.ExecuteAsync(sql, new { UserId = userId, AssignmentId = assignmentId, UpdatedAt = updatedAt, Id = id }) == 1;
    }
}
