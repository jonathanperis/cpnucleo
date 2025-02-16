namespace Domain.Repositories;

public interface IUserProjectRepository
{
    Task<bool> CreateUserProject(UserProject userProject);
    Task<UserProjectDto?> GetUserProjectById(Ulid id);
    Task<List<UserProjectDto>?> ListUserProjects();
    Task<bool> RemoveUserProject(Ulid id);
    Task<bool> UpdateUserProject(Ulid id, Ulid userId, Ulid projectId);
}
