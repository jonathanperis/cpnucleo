namespace Application.UseCases.Project.RemoveProject;

public sealed record RemoveProjectCommand(Ulid Id) : BaseCommand, IRequest<OperationResult>;
