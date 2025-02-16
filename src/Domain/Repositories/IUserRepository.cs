namespace Domain.Repositories;

public interface IUserRepository
{
    Task<bool> CreateUser(User user);
    Task<UserDto?> GetUserById(Ulid id);
    Task<List<UserDto>?> ListUsers();
    Task<bool> RemoveUser(Ulid id);
    Task<bool> UpdateUser(Ulid id, string name, string password, string salt);
}
