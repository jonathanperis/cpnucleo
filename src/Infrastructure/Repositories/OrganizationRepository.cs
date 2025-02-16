namespace Infrastructure.Repositories;

//[DapperAot]
public class OrganizationRepository(IConfiguration configuration) : IOrganizationRepository
{
    public async Task<bool> CreateOrganization(Organization organization)
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           INSERT INTO "Organizations" ("Id", "Name", "Description", "CreatedAt", "Active")
                           VALUES (@Id, @Name, @Description, @CreatedAt, @Active);
                           """;

        return await connection.ExecuteAsync(sql, new { organization.Id, organization.Name, organization.Description, organization.CreatedAt, organization.Active }) == 1;
    }

    public async Task<OrganizationDto?> GetOrganizationById(Ulid id)
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           SELECT "Id", "Name", "Description", "CreatedAt", "UpdatedAt", "Active"
                           FROM "Organizations"
                           WHERE "Id" = @Id AND "Active" = true;
                           """;

        return await connection.QueryFirstOrDefaultAsync<OrganizationDto>(sql, new { Id = id });
    }

    public async Task<List<OrganizationDto>?> ListOrganization()
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           SELECT "Id", "Name", "Description", "CreatedAt", "UpdatedAt", "Active"
                           FROM "Organizations"
                           WHERE "Active" = true;
                           """;

        return (await connection.QueryAsync<OrganizationDto>(sql)).AsList();
    }

    public async Task<bool> RemoveOrganization(Ulid id)
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           UPDATE "Organizations"
                           SET "Active" = 0, "DeletedAt" = @DeletedAt
                           WHERE "Id" = @Id;
                           """;

        var deletedAt = DateTime.UtcNow;

        return await connection.ExecuteAsync(sql, new { DeletedAt = deletedAt, Id = id }) == 1;
    }

    public async Task<bool> UpdateOrganization(Ulid id, string name, string description)
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           UPDATE "Organizations"
                           SET "Name" = @Name, "Description" = @Description, "UpdatedAt" = @UpdatedAt
                           WHERE "Id" = @Id;
                           """;

        var updatedAt = DateTime.UtcNow;

        return await connection.ExecuteAsync(sql, new { Name = name, Description = description, UpdatedAt = updatedAt, Id = id }) == 1;
    }
}
