namespace Infrastructure.Repositories;

public class WorkflowRepository(NpgsqlConnection connection) : IWorkflowRepository
{
    public async Task<bool> CreateWorkflow(Workflow workflow)
    {
        const string sql = """
                           INSERT INTO "Workflows" ("Id", "Name", "Order", "CreatedAt", "Active")
                           VALUES (@Id, @Name, @Order, @CreatedAt, @Active);
                           """;

        return await connection.ExecuteAsync(sql, new { workflow.Id, workflow.Name, workflow.Order, workflow.CreatedAt, workflow.Active }) == 1;
    }

    public async Task<Workflow?> GetWorkflowById(Guid id)
    {
        const string sql = """
                           SELECT "Id", "Name", "Order", "CreatedAt", "UpdatedAt", "Active"
                           FROM "Workflows"
                           WHERE "Id" = @Id AND "Active" = true;
                           """;

        return await connection.QueryFirstOrDefaultAsync<Workflow>(sql, new { Id = id });
    }

    public async Task<List<Workflow?>?> ListWorkflow()
    {
        const string sql = """
                           SELECT "Id", "Name", "Order", "CreatedAt", "UpdatedAt", "Active"
                           FROM "Workflows"
                           WHERE "Active" = true;
                           """;

        return (await connection.QueryAsync<Workflow?>(sql)).AsList();
    }

    public async Task<bool> RemoveWorkflow(Guid id)
    {
        const string sql = """
                           UPDATE "Workflows"
                           SET "Active" = false, "DeletedAt" = @DeletedAt
                           WHERE "Id" = @Id;
                           """;

        var deletedAt = DateTime.UtcNow;

        return await connection.ExecuteAsync(sql, new { DeletedAt = deletedAt, Id = id }) == 1;
    }

    public async Task<bool> UpdateWorkflow(Guid id, string name, int order)
    {
        const string sql = """
                           UPDATE "Workflows"
                           SET "Name" = @Name, "Order" = @Order, "UpdatedAt" = @UpdatedAt
                           WHERE "Id" = @Id;
                           """;

        var updatedAt = DateTime.UtcNow;

        return await connection.ExecuteAsync(sql, new { Name = name, Order = order, UpdatedAt = updatedAt, Id = id }) == 1;
    }
}
