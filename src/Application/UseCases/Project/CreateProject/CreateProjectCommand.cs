namespace Application.UseCases.Project.CreateProject;

public sealed record CreateProjectCommand(string Name, Ulid OrganizationId, Ulid Id = default) : BaseCommand, IRequest<OperationResult>;
