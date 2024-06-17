namespace Application.UseCases.Project.UpdateProject;

public sealed record UpdateProjectCommand(Ulid Id, string Name, Ulid OrganizationId) : BaseCommand, IRequest<OperationResult>;
