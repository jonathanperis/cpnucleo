namespace GrpcServer.Handlers.Project;

public sealed class CreateProjectHandler(Application.Features.Projects.CreateProject.CreateProjectHandler handler) : ICommandHandler<CreateProjectCommand, CreateProjectResult>
{
    public async Task<CreateProjectResult> ExecuteAsync(CreateProjectCommand command, CancellationToken cancellationToken)
    {
        var result = await handler.ExecuteAsync(
            new Application.Features.Projects.CreateProject.CreateProjectRequest(command.Id, command.Name, command.OrganizationId),
            cancellationToken);

        return new CreateProjectResult
        {
            Success = result.Success,
            Message = result.Message,
            Project = result.Project is null
                ? null
                : new ProjectDto
                {
                    Id = result.Project.Id,
                    CreatedAt = result.Project.CreatedAt,
                    Name = result.Project.Name,
                    OrganizationId = result.Project.OrganizationId
                }
        };
    }
}
