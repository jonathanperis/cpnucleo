namespace Domain.Repositories;

public interface IProjectRepository
{
    Task<bool> CreateProject(Project project);
    Task<Project?> GetProjectById(Guid id);
    Task<List<Project?>?> ListProjects();
    Task<bool> RemoveProject(Guid id);
    Task<bool> UpdateProject(Guid id, string name, Guid organizationId);
}
