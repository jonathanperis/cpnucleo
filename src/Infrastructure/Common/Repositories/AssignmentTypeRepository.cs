namespace Infrastructure.Common.Repositories;

//[DapperAot]
public class AssignmentTypeRepository(IConfiguration configuration) : IAssignmentTypeRepository
{
    public async Task<bool> CreateAssignmentType(AssignmentType assignmentType)
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           INSERT INTO "AssignmentType" ("Id", "Name", "CreatedAt", "Active")
                           VALUES (@Id, @Name, @CreatedAt, @Active);
                           """;

        return await connection.ExecuteAsync(sql, new { assignmentType.Id, assignmentType.Name, assignmentType.CreatedAt, assignmentType.Active }) == 1;
    }

    public async Task<AssignmentTypeDto?> GetAssignmentTypeById(Ulid id)
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           SELECT "Id", "Name", "CreatedAt", "UpdatedAt", "Active"
                           FROM "AssignmentType"
                           WHERE "Id" = @Id AND "Active" = 1;
                           """;

        return await connection.QueryFirstOrDefaultAsync<AssignmentTypeDto>(sql, new { Id = id });
    }

    public async Task<List<AssignmentTypeDto>?> ListAssignmentTypes()
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           SELECT "Id", "Name", "CreatedAt", "UpdatedAt", "Active"
                           FROM "AssignmentType"
                           WHERE "Active" = 1;
                           """;

        return (await connection.QueryAsync<AssignmentTypeDto>(sql)).AsList();
    }

    public async Task<bool> RemoveAssignmentType(Ulid id)
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           UPDATE "AssignmentType"
                           SET "Active" = 0, "DeletedAt" = @DeletedAt
                           WHERE "Id" = @Id;
                           """;

        var deletedAt = DateTime.UtcNow;

        return await connection.ExecuteAsync(sql, new { DeletedAt = deletedAt, Id = id }) == 1;
    }

    public async Task<bool> UpdateAssignmentType(Ulid id, string name)
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           UPDATE "AssignmentType"
                           SET "Name" = @Name, "UpdatedAt" = @UpdatedAt
                           WHERE "Id" = @Id;
                           """;

        var updatedAt = DateTime.UtcNow;

        return await connection.ExecuteAsync(sql, new { Name = name, UpdatedAt = updatedAt, Id = id }) == 1;
    }
}
