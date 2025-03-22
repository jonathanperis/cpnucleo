namespace Application.UseCases.Project.UpdateProject;

public sealed record UpdateProjectCommand(Guid Id, string Name, Guid OrganizationId) : BaseCommand, IRequest<OperationResult>;
