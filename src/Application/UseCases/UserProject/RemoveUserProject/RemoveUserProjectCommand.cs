namespace Application.UseCases.UserProject.RemoveUserProject;

public sealed record RemoveUserProjectCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;
