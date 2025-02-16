namespace Infrastructure.Repositories;

public class UserRepository(IConfiguration configuration) : IUserRepository
{
    public async Task<bool> CreateUser(User user)
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           INSERT INTO "User" ("Id", "Name", "Login", "Password", "Salt", "CreatedAt", "Active")
                           VALUES (@Id, @Name, @Login, @Password, @Salt, @CreatedAt, @Active);
                           """;

        return await connection.ExecuteAsync(sql, new { user.Id, user.Name, user.Login, user.Password, user.Salt, user.CreatedAt, user.Active }) == 1;
    }

    public async Task<UserDto?> GetUserById(Ulid id)
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           SELECT "Id", "Name", "Login", "CreatedAt", "UpdatedAt", "Active"
                           FROM "User"
                           WHERE "Id" = @Id AND "Active" = 1;
                           """;

        return await connection.QueryFirstOrDefaultAsync<UserDto>(sql, new { Id = id });
    }

    public async Task<List<UserDto>?> ListUsers()
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           SELECT "Id", "Name", "Login", "CreatedAt", "UpdatedAt", "Active"
                           FROM "User"
                           WHERE "Active" = 1;
                           """;

        return (await connection.QueryAsync<UserDto>(sql)).AsList();
    }

    public async Task<bool> RemoveUser(Ulid id)
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           UPDATE "User"
                           SET "Active" = 0, "DeletedAt" = @DeletedAt
                           WHERE "Id" = @Id;
                           """;

        var deletedAt = DateTime.UtcNow;

        return await connection.ExecuteAsync(sql, new { DeletedAt = deletedAt, Id = id }) == 1;
    }

    public async Task<bool> UpdateUser(Ulid id, string name, string password, string salt)
    {
        await using var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

        const string sql = """
                           UPDATE "User"
                           SET "Name" = @Name, "Password" = @Password, "Salt" = @Salt, "UpdatedAt" = @UpdatedAt
                           WHERE "Id" = @Id;
                           """;

        var updatedAt = DateTime.UtcNow;

        return await connection.ExecuteAsync(sql, new { Name = name, Password = password, Salt = salt, UpdatedAt = updatedAt, Id = id }) == 1;
    }
}
