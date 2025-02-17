namespace Application.UseCases.UserProject.UpdateUserProject;

public sealed record UpdateUserProjectCommand(Guid Id, Guid UserId, Guid ProjectId) : BaseCommand, IRequest<OperationResult>;
