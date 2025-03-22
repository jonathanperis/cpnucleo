namespace Application.UseCases.Project.CreateProject;

public sealed record CreateProjectCommand(string Name, Guid OrganizationId, Guid Id = default) : BaseCommand, IRequest<OperationResult>;
