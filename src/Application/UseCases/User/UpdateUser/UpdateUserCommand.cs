namespace Application.UseCases.User.UpdateUser;

public sealed record UpdateUserCommand(Ulid Id, string Name, string Password) : BaseCommand, IRequest<OperationResult>;
