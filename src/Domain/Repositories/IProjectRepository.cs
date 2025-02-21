namespace Domain.Repositories;

public interface IProjectRepository
{
    Task<bool> CreateProject(Project project);
    Task<Project?> GetProjectById(Guid id);
    Task<PaginatedResult<Project?>> ListProjects(PaginationParams pagination);
    Task<bool> RemoveProject(Guid id);
    Task<bool> UpdateProject(Guid id, string name, Guid organizationId);
}
