namespace Application.UseCases.UserProject.RemoveUserProject;

public sealed record RemoveUserProjectCommand(Ulid Id) : BaseCommand, IRequest<OperationResult>;
