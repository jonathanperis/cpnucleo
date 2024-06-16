namespace Application.UseCases.Project.CreateProject;

public sealed record CreateProjectCommand(string Name, Ulid SystemId, Ulid Id = default) : BaseCommand, IRequest<OperationResult>;
