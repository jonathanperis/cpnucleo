namespace Application.UseCases.UserProject.CreateUserProject;

public sealed record CreateUserProjectCommand(Guid UserId, Guid ProjectId, Guid Id = default) : BaseCommand, IRequest<OperationResult>;
