namespace Application.UseCases.User.RemoveUser;

public sealed record RemoveUserCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;
