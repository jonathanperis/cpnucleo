namespace Infrastructure.Repositories;

//[DapperAot]
public class OrganizationRepository(NpgsqlConnection connection) : IOrganizationRepository
{
    public async Task<bool> CreateOrganization(Organization organization)
    {
        const string sql = """
                           INSERT INTO "Organizations" ("Id", "Name", "Description", "CreatedAt", "Active")
                           VALUES (@Id, @Name, @Description, @CreatedAt, @Active);
                           """;

        return await connection.ExecuteAsync(sql, new { organization.Id, organization.Name, organization.Description, organization.CreatedAt, organization.Active }) == 1;
    }

    public async Task<Organization?> GetOrganizationById(Guid id)
    {
        const string sql = """
                           SELECT "Id", "Name", "Description", "CreatedAt", "UpdatedAt", "Active"
                           FROM "Organizations"
                           WHERE "Id" = @Id AND "Active" = true;
                           """;

        return await connection.QueryFirstOrDefaultAsync<Organization>(sql, new { Id = id });
    }

    public async Task<List<Organization?>?> ListOrganization()
    {
        const string sql = """
                           SELECT "Id", "Name", "Description", "CreatedAt", "UpdatedAt", "Active"
                           FROM "Organizations"
                           WHERE "Active" = true;
                           """;

        return (await connection.QueryAsync<Organization?>(sql)).AsList();
    }

    public async Task<bool> RemoveOrganization(Guid id)
    {
        const string sql = """
                           UPDATE "Organizations"
                           SET "Active" = 0, "DeletedAt" = @DeletedAt
                           WHERE "Id" = @Id;
                           """;

        var deletedAt = DateTime.UtcNow;

        return await connection.ExecuteAsync(sql, new { DeletedAt = deletedAt, Id = id }) == 1;
    }

    public async Task<bool> UpdateOrganization(Guid id, string name, string description)
    {
        const string sql = """
                           UPDATE "Organizations"
                           SET "Name" = @Name, "Description" = @Description, "UpdatedAt" = @UpdatedAt
                           WHERE "Id" = @Id;
                           """;

        var updatedAt = DateTime.UtcNow;

        return await connection.ExecuteAsync(sql, new { Name = name, Description = description, UpdatedAt = updatedAt, Id = id }) == 1;
    }
}
