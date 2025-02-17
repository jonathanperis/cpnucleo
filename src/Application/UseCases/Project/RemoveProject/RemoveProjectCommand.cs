namespace Application.UseCases.Project.RemoveProject;

public sealed record RemoveProjectCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;
