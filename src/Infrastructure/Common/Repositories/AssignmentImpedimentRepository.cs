namespace Infrastructure.Common.Repositories;

public class AssignmentImpedimentRepository(IConfiguration configuration) : IAssignmentImpedimentRepository
{
    public async Task<bool> CreateAssignmentImpediment(AssignmentImpediment assignmentImpediment)
    {
        await using var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           INSERT INTO [AssignmentImpediment] ([Id], [Description], [AssignmentId], [ImpedimentId], [CreatedAt], [Active])
                           VALUES (@Id, @Description, @AssignmentId, @ImpedimentId, @CreatedAt, @Active);
                           """;

        return await connection.ExecuteAsync(sql, new { assignmentImpediment.Id, assignmentImpediment.Description, assignmentImpediment.AssignmentId, assignmentImpediment.ImpedimentId, assignmentImpediment.CreatedAt, assignmentImpediment.Active }) == 1;
    }

    public async Task<AssignmentImpedimentDto?> GetAssignmentImpedimentById(Ulid id)
    {
        await using var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           SELECT [Id], [Description], [AssignmentId], [ImpedimentId], [CreatedAt], [UpdatedAt], [Active]
                           FROM [AssignmentImpediment]
                           WHERE [Id] = @Id AND [Active] = 1;
                           """;

        return await connection.QueryFirstOrDefaultAsync<AssignmentImpedimentDto>(sql, new { Id = id });
    }

    public async Task<List<AssignmentImpedimentDto>?> ListAssignmentImpediments()
    {
        await using var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           SELECT [Id], [Description], [AssignmentId], [ImpedimentId], [CreatedAt], [UpdatedAt], [Active]
                           FROM [AssignmentImpediment]
                           WHERE [Active] = 1;
                           """;

        return (await connection.QueryAsync<AssignmentImpedimentDto>(sql)).AsList();
    }

    public async Task<bool> RemoveAssignmentImpediment(Ulid id)
    {
        await using var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           UPDATE [AssignmentImpediment]
                           SET [Active] = 0, [DeletedAt] = @DeletedAt
                           WHERE [Id] = @Id;
                           """;

        var deletedAt = DateTime.UtcNow;

        return await connection.ExecuteAsync(sql, new { DeletedAt = deletedAt, Id = id }) == 1;
    }

    public async Task<bool> UpdateAssignmentImpediment(Ulid id, string description, Ulid assignmentId, Ulid impedimentId)
    {
        await using var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           UPDATE [AssignmentImpediment]
                           SET [Description] = @Description, [AssignmentId] = @AssignmentId, [ImpedimentId] = @ImpedimentId, [UpdatedAt] = @UpdatedAt
                           WHERE [Id] = @Id;
                           """;

        var updatedAt = DateTime.UtcNow;

        return await connection.ExecuteAsync(sql, new { Description = description, AssignmentId = assignmentId, ImpedimentId = impedimentId, UpdatedAt = updatedAt, Id = id }) == 1;
    }
}
