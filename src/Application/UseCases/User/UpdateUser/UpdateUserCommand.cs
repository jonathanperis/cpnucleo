namespace Application.UseCases.User.UpdateUser;

public sealed record UpdateUserCommand(Guid Id, string Name, string Password) : BaseCommand, IRequest<OperationResult>;
