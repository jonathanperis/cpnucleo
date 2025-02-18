namespace Infrastructure.Repositories;

//[DapperAot]
public class AssignmentTypeRepository(NpgsqlConnection connection) : IAssignmentTypeRepository
{
    public async Task<bool> CreateAssignmentType(AssignmentType assignmentType)
    {
        const string sql = """
                           INSERT INTO "AssignmentTypes" ("Id", "Name", "CreatedAt", "Active")
                           VALUES (@Id, @Name, @CreatedAt, @Active);
                           """;

        return await connection.ExecuteAsync(sql, new { assignmentType.Id, assignmentType.Name, assignmentType.CreatedAt, assignmentType.Active }) == 1;
    }

    public async Task<AssignmentType?> GetAssignmentTypeById(Guid id)
    {
        const string sql = """
                           SELECT "Id", "Name", "CreatedAt", "UpdatedAt", "Active"
                           FROM "AssignmentTypes"
                           WHERE "Id" = @Id AND "Active" = true;
                           """;

        return await connection.QueryFirstOrDefaultAsync<AssignmentType>(sql, new { Id = id });
    }

    public async Task<List<AssignmentType?>?> ListAssignmentTypes()
    {
        const string sql = """
                           SELECT "Id", "Name", "CreatedAt", "UpdatedAt", "Active"
                           FROM "AssignmentTypes"
                           WHERE "Active" = true;
                           """;

        return (await connection.QueryAsync<AssignmentType?>(sql)).AsList();
    }

    public async Task<bool> RemoveAssignmentType(Guid id)
    {
        const string sql = """
                           UPDATE "AssignmentTypes"
                           SET "Active" = 0, "DeletedAt" = @DeletedAt
                           WHERE "Id" = @Id;
                           """;

        var deletedAt = DateTime.UtcNow;

        return await connection.ExecuteAsync(sql, new { DeletedAt = deletedAt, Id = id }) == 1;
    }

    public async Task<bool> UpdateAssignmentType(Guid id, string name)
    {
        const string sql = """
                           UPDATE "AssignmentTypes"
                           SET "Name" = @Name, "UpdatedAt" = @UpdatedAt
                           WHERE "Id" = @Id;
                           """;

        var updatedAt = DateTime.UtcNow;

        return await connection.ExecuteAsync(sql, new { Name = name, UpdatedAt = updatedAt, Id = id }) == 1;
    }
}
