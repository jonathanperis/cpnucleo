namespace Infrastructure.Common.Repositories;

//[DapperAot]
public class WorkflowRepository(IConfiguration configuration) : IWorkflowRepository
{
    public async Task<bool> CreateWorkflow(Workflow workflow)
    {
        await using var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = @"
                            INSERT INTO [Workflow] ([Id], [Name], [Order], [CreatedAt], [Active])
                            VALUES (@Id, @Name, @Order, @CreatedAt, @Active);
                            ";

        return await connection.ExecuteAsync(sql, new { workflow.Id, workflow.Name, workflow.Order, workflow.CreatedAt, workflow.Active }) == 1;
    }

    public async Task<WorkflowDto?> GetWorkflowById(Ulid id)
    {
        await using var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = @"
                            SELECT [Id], [Name], [Order], [CreatedAt], [UpdatedAt], [Active]
                            FROM [Workflow]
                            WHERE [Id] = @Id AND [Active] = 1;
                            ";

        return await connection.QueryFirstOrDefaultAsync<WorkflowDto>(sql, new { Id = id });
    }

    public async Task<List<WorkflowDto>?> ListWorkflow()
    {
        await using var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = @"
                            SELECT [Id], [Name], [Order], [CreatedAt], [UpdatedAt], [Active]
                            FROM [Workflow]
                            WHERE [Active] = 1;
                            ";

        return (await connection.QueryAsync<WorkflowDto>(sql)).AsList();
    }

    public async Task<bool> RemoveWorkflow(Ulid id)
    {
        await using var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = @"
                            UPDATE [Workflow]
                            SET [Active] = 0, [DeletedAt] = @DeletedAt
                            WHERE [Id] = @Id;
                            ";

        var deletedAt = DateTime.UtcNow;

        return await connection.ExecuteAsync(sql, new { DeletedAt = deletedAt, Id = id }) == 1;
    }

    public async Task<bool> UpdateWorkflow(Ulid id, string name, byte order)
    {
        await using var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = @"
                            UPDATE [Workflow]
                            SET [Name] = @Name, [Order] = @Order, [UpdatedAt] = @UpdatedAt
                            WHERE [Id] = @Id;
                            ";

        var updatedAt = DateTime.UtcNow;

        return await connection.ExecuteAsync(sql, new { Name = name, Order = order, UpdatedAt = updatedAt, Id = id }) == 1;
    }
}
