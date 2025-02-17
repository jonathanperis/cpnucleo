namespace Domain.Repositories;

public interface IUserProjectRepository
{
    Task<bool> CreateUserProject(UserProject userProject);
    Task<UserProject?> GetUserProjectById(Guid id);
    Task<List<UserProject>?> ListUserProjects();
    Task<bool> RemoveUserProject(Guid id);
    Task<bool> UpdateUserProject(Guid id, Guid userId, Guid projectId);
}
