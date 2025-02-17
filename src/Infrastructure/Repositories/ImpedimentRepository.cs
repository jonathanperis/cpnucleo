namespace Infrastructure.Repositories;

public class ImpedimentRepository(IConfiguration configuration) : IImpedimentRepository
{
    public async Task<bool> CreateImpediment(Impediment impediment)
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           INSERT INTO "Impediments" ("Id", "Name", "CreatedAt", "Active")
                           VALUES (@Id, @Name, @CreatedAt, @Active);
                           """;

        return await connection.ExecuteAsync(sql, new { impediment.Id, impediment.Name, impediment.CreatedAt, impediment.Active }) == 1;
    }

    public async Task<Impediment?> GetImpedimentById(Guid id)
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           SELECT "Id", "Name", "CreatedAt", "UpdatedAt", "Active"
                           FROM "Impediments"
                           WHERE "Id" = @Id AND "Active" = true;
                           """;

        return await connection.QueryFirstOrDefaultAsync<Impediment>(sql, new { Id = id });
    }

    public async Task<List<Impediment>?> ListImpediments()
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           SELECT "Id", "Name", "CreatedAt", "UpdatedAt", "Active"
                           FROM "Impediments"
                           WHERE "Active" = true;
                           """;

        return (await connection.QueryAsync<Impediment>(sql)).AsList();
    }

    public async Task<bool> RemoveImpediment(Guid id)
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           UPDATE "Impediments"
                           SET "Active" = 0, "DeletedAt" = @DeletedAt
                           WHERE "Id" = @Id;
                           """;

        var deletedAt = DateTime.UtcNow;

        return await connection.ExecuteAsync(sql, new { DeletedAt = deletedAt, Id = id }) == 1;
    }

    public async Task<bool> UpdateImpediment(Guid id, string name)
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           UPDATE "Impediments"
                           SET "Name" = @Name, "UpdatedAt" = @UpdatedAt
                           WHERE "Id" = @Id;
                           """;

        var updatedAt = DateTime.UtcNow;

        return await connection.ExecuteAsync(sql, new { Name = name, UpdatedAt = updatedAt, Id = id }) == 1;
    }
}
