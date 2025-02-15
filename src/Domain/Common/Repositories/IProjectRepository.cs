namespace Domain.Common.Repositories;

public interface IProjectRepository
{
    Task<bool> CreateProject(Project project);
    Task<ProjectDto?> GetProjectById(Ulid id);
    Task<List<ProjectDto>?> ListProjects();
    Task<bool> RemoveProject(Ulid id);
    Task<bool> UpdateProject(Ulid id, string name, Ulid organizationId);
}
