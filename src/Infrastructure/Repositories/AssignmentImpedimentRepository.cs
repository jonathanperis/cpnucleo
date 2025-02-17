namespace Infrastructure.Repositories;

public class AssignmentImpedimentRepository(IConfiguration configuration) : IAssignmentImpedimentRepository
{
    public async Task<bool> CreateAssignmentImpediment(AssignmentImpediment assignmentImpediment)
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           INSERT INTO "AssignmentImpediments" ("Id", "Description", "AssignmentId", "ImpedimentId", "CreatedAt", "Active")
                           VALUES (@Id, @Description, @AssignmentId, @ImpedimentId, @CreatedAt, @Active);
                           """;

        return await connection.ExecuteAsync(sql, new { assignmentImpediment.Id, assignmentImpediment.Description, assignmentImpediment.AssignmentId, assignmentImpediment.ImpedimentId, assignmentImpediment.CreatedAt, assignmentImpediment.Active }) == 1;
    }

    public async Task<AssignmentImpediment?> GetAssignmentImpedimentById(Guid id)
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           SELECT "Id", "Description", "AssignmentId", "ImpedimentId", "CreatedAt", "UpdatedAt", "Active"
                           FROM "AssignmentImpediments"
                           WHERE "Id" = @Id AND "Active" = true;
                           """;

        return await connection.QueryFirstOrDefaultAsync<AssignmentImpediment>(sql, new { Id = id });
    }

    public async Task<List<AssignmentImpediment>?> ListAssignmentImpediments()
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           SELECT "Id", "Description", "AssignmentId", "ImpedimentId", "CreatedAt", "UpdatedAt", "Active"
                           FROM "AssignmentImpediments"
                           WHERE "Active" = true;
                           """;

        return (await connection.QueryAsync<AssignmentImpediment>(sql)).AsList();
    }

    public async Task<bool> RemoveAssignmentImpediment(Guid id)
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           UPDATE "AssignmentImpediments"
                           SET "Active" = 0, "DeletedAt" = @DeletedAt
                           WHERE "Id" = @Id;
                           """;

        var deletedAt = DateTime.UtcNow;

        return await connection.ExecuteAsync(sql, new { DeletedAt = deletedAt, Id = id }) == 1;
    }

    public async Task<bool> UpdateAssignmentImpediment(Guid id, string description, Guid assignmentId, Guid impedimentId)
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           UPDATE "AssignmentImpediments"
                           SET "Description" = @Description, "AssignmentId" = @AssignmentId, "ImpedimentId" = @ImpedimentId, "UpdatedAt" = @UpdatedAt
                           WHERE "Id" = @Id;
                           """;

        var updatedAt = DateTime.UtcNow;

        return await connection.ExecuteAsync(sql, new { Description = description, AssignmentId = assignmentId, ImpedimentId = impedimentId, UpdatedAt = updatedAt, Id = id }) == 1;
    }
}
