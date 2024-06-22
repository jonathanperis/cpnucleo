namespace Application.UseCases.User.CreateUser;

public sealed record CreateUserCommand(string Name, string Login, string Password, Ulid Id = default) : BaseCommand, IRequest<OperationResult>;
