namespace Application.Features.Projects;

public sealed record ProjectDetails(Guid Id, DateTime CreatedAt, string? Name, Guid OrganizationId)
{
    public static ProjectDetails FromEntity(Project project)
    {
        return new ProjectDetails(project.Id, project.CreatedAt, project.Name, project.OrganizationId);
    }
}
