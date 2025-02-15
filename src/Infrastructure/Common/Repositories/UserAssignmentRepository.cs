namespace Infrastructure.Common.Repositories;

public class UserAssignmentRepository(IConfiguration configuration) : IUserAssignmentRepository
{
    public async Task<bool> CreateUserAssignment(UserAssignment userAssignment)
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           INSERT INTO "UserAssignment" ("Id", "UserId", "AssignmentId", "CreatedAt", "Active")
                           VALUES (@Id, @UserId, @AssignmentId, @CreatedAt, @Active);
                           """;

        return await connection.ExecuteAsync(sql, new { userAssignment.Id, userAssignment.UserId, userAssignment.AssignmentId, userAssignment.CreatedAt, userAssignment.Active }) == 1;
    }

    public async Task<UserAssignmentDto?> GetUserAssignmentById(Ulid id)
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           SELECT "Id", "UserId", "AssignmentId", "CreatedAt", "UpdatedAt", "Active"
                           FROM "UserAssignment"
                           WHERE "Id" = @Id AND "Active" = 1;
                           """;

        return await connection.QueryFirstOrDefaultAsync<UserAssignmentDto>(sql, new { Id = id });
    }

    public async Task<List<UserAssignmentDto>?> ListUserAssignments()
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           SELECT "Id", "UserId", "AssignmentId", "CreatedAt", "UpdatedAt", "Active"
                           FROM "UserAssignment"
                           WHERE "Active" = 1;
                           """;

        return (await connection.QueryAsync<UserAssignmentDto>(sql)).AsList();
    }

    public async Task<bool> RemoveUserAssignment(Ulid id)
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           UPDATE "UserAssignment"
                           SET "Active" = 0, "DeletedAt" = @DeletedAt
                           WHERE "Id" = @Id;
                           """;

        var deletedAt = DateTime.UtcNow;

        return await connection.ExecuteAsync(sql, new { DeletedAt = deletedAt, Id = id }) == 1;
    }

    public async Task<bool> UpdateUserAssignment(Ulid id, Ulid userId, Ulid assignmentId)
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           UPDATE "UserAssignment"
                           SET "UserId" = @UserId, "AssignmentId" = @AssignmentId, "UpdatedAt" = @UpdatedAt
                           WHERE "Id" = @Id;
                           """;

        var updatedAt = DateTime.UtcNow;

        return await connection.ExecuteAsync(sql, new { UserId = userId, AssignmentId = assignmentId, UpdatedAt = updatedAt, Id = id }) == 1;
    }
}
