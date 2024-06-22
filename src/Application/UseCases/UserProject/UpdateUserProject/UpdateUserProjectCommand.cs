namespace Application.UseCases.UserProject.UpdateUserProject;

public sealed record UpdateUserProjectCommand(Ulid Id, Ulid UserId, Ulid ProjectId) : BaseCommand, IRequest<OperationResult>;
