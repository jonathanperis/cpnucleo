namespace Infrastructure.Repositories;

//[DapperAot]
public class AssignmentRepository(IConfiguration configuration) : IAssignmentRepository
{
    public async Task<bool> CreateAssignment(Assignment assignment)
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = @"
                            INSERT INTO ""Assignment"" (""Id"", ""Name"", ""Description"", ""StartDate"", ""EndDate"", ""AmountHours"", ""ProjectId"", ""WorkflowId"", ""UserId"", ""AssignmentTypeId"", ""CreatedAt"", ""Active"")
                            VALUES (@Id, @Name, @Description, @StartDate, @EndDate, @AmountHours, @ProjectId, @WorkflowId, @UserId, @AssignmentTypeId, @CreatedAt, @Active);
                            ";

        return await connection.ExecuteAsync(sql, new { assignment.Id, assignment.Name, assignment.Description, assignment.StartDate, assignment.EndDate, assignment.AmountHours, assignment.ProjectId, assignment.WorkflowId, assignment.UserId, assignment.AssignmentTypeId, assignment.CreatedAt, assignment.Active }) == 1;
    }

    public async Task<AssignmentDto?> GetAssignmentById(Ulid id)
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = @"
                            SELECT ""Id"", ""Name"", ""Description"", ""StartDate"", ""EndDate"", ""AmountHours"", ""ProjectId"", ""WorkflowId"", ""UserId"", ""AssignmentTypeId"", ""CreatedAt"", ""UpdatedAt"", ""Active""
                            FROM ""Assignment""
                            WHERE ""Id"" = @Id AND ""Active"" = 1;
                            ";

        return await connection.QueryFirstOrDefaultAsync<AssignmentDto>(sql, new { Id = id });
    }

    public async Task<List<AssignmentDto>?> ListAssignments()
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = @"
                            SELECT ""Id"", ""Name"", ""Description"", ""StartDate"", ""EndDate"", ""AmountHours"", ""ProjectId"", ""WorkflowId"", ""UserId"", ""AssignmentTypeId"", ""CreatedAt"", ""UpdatedAt"", ""Active""
                            FROM ""Assignment""
                            WHERE ""Active"" = 1;
                            ";

        return (await connection.QueryAsync<AssignmentDto>(sql)).AsList();
    }

    public async Task<bool> RemoveAssignment(Ulid id)
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = @"
                            UPDATE ""Assignment""
                            SET ""Active"" = 0, ""DeletedAt"" = @DeletedAt
                            WHERE ""Id"" = @Id;
                            ";

        var deletedAt = DateTime.UtcNow;

        return await connection.ExecuteAsync(sql, new { DeletedAt = deletedAt, Id = id }) == 1;
    }

    public async Task<bool> UpdateAssignment(Ulid id, string name, string description, DateTime startDate, DateTime endDate, byte amountHours, Ulid projectId, Ulid workflowId, Ulid userId, Ulid assignmentTypeId)
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = @"
                            UPDATE ""Assignment""
                            SET ""Name"" = @Name, ""Description"" = @Description, ""StartDate"" = @StartDate, ""EndDate"" = @EndDate, ""AmountHours"" = @AmountHours, ""ProjectId"" = @ProjectId, ""WorkflowId"" = @WorkflowId, ""UserId"" = @UserId, ""AssignmentTypeId"" = @AssignmentTypeId, ""UpdatedAt"" = @UpdatedAt
                            WHERE ""Id"" = @Id;
                            ";

        var updatedAt = DateTime.UtcNow;

        return await connection.ExecuteAsync(sql, new { Name = name, Description = description, StartDate = startDate, EndDate = endDate, AmountHours = amountHours, ProjectId = projectId, WorkflowId = workflowId, UserId = userId, AssignmentTypeId = assignmentTypeId, UpdatedAt = updatedAt, Id = id }) == 1;
    }
}
