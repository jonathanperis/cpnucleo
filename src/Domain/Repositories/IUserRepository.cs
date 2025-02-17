namespace Domain.Repositories;

public interface IUserRepository
{
    Task<bool> CreateUser(User user);
    Task<User?> GetUserById(Guid id);
    Task<List<User>?> ListUsers();
    Task<bool> RemoveUser(Guid id);
    Task<bool> UpdateUser(Guid id, string name, string password, string salt);
}
