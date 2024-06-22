namespace Application.UseCases.User.RemoveUser;

public sealed record RemoveUserCommand(Ulid Id) : BaseCommand, IRequest<OperationResult>;
