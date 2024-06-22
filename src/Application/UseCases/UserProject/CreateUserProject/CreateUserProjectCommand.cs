namespace Application.UseCases.UserProject.CreateUserProject;

public sealed record CreateUserProjectCommand(Ulid UserId, Ulid ProjectId, Ulid Id = default) : BaseCommand, IRequest<OperationResult>;
