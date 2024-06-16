namespace Infrastructure.Common.Repositories;

//[DapperAot]
public class SystemRepository : ISystemRepository
{
    private readonly IConfiguration _configuration;

    public SystemRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> CreateSystem(Domain.Entities.System system)
    {
        await using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

        const string sql = @"
                            INSERT INTO [System] ([Id], [Name], [Description], [CreatedAt], [Active])
                            VALUES (@Id, @Name, @Description, @CreatedAt, @Active);
                            ";

        return await connection.ExecuteAsync(sql, new { system.Id, system.Name, system.Description, system.CreatedAt, system.Active }) == 1;
    }

    public async Task<SystemDto?> GetSystemById(Ulid id)
    {
        await using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

        const string sql = @"
                            SELECT [Id], [Name], [Description], [CreatedAt], [UpdatedAt], [Active]
                            FROM [System]
                            WHERE [Id] = @Id AND [Active] = 1;
                            ";

        return await connection.QueryFirstOrDefaultAsync<SystemDto>(sql, new { Id = id });
    }

    public async Task<List<SystemDto>?> ListSystem()
    {
        await using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

        const string sql = @"
                            SELECT [Id], [Name], [Description], [CreatedAt], [UpdatedAt], [Active]
                            FROM [System]
                            WHERE [Active] = 1;
                            ";

        return (await connection.QueryAsync<SystemDto>(sql)).AsList();
    }

    public async Task<bool> RemoveSystem(Ulid id)
    {
        await using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

        const string sql = @"
                            UPDATE [System]
                            SET [Active] = 0, [DeletedAt] = @DeletedAt
                            WHERE [Id] = @Id;
                            ";

        var deletedAt = DateTime.UtcNow;

        return await connection.ExecuteAsync(sql, new { DeletedAt = deletedAt, Id = id }) == 1;
    }

    public async Task<bool> UpdateSystem(Ulid id, string name, string description)
    {
        await using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

        const string sql = @"
                            UPDATE [System]
                            SET [Name] = @Name, [Description] = @Description, [UpdatedAt] = @UpdatedAt
                            WHERE [Id] = @Id;
                            ";

        var updatedAt = DateTime.UtcNow;

        return await connection.ExecuteAsync(sql, new { Name = name, Description = description, UpdatedAt = updatedAt, Id = id }) == 1;
    }
}
