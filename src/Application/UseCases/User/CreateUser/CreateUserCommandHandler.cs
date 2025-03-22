namespace Application.UseCases.User.CreateUser;

// EF Core
public sealed class CreateUserCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<CreateUserCommand, OperationResult>
{
    public async ValueTask<OperationResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = Domain.Entities.User.Create(request.Name, request.Login, request.Password, request.Id);

        if (dbContext.Users is not null)
            await dbContext.Users.AddAsync(user, cancellationToken);

        var result = await dbContext.SaveChangesAsync(cancellationToken);

        return result ? OperationResult.Success : OperationResult.Failed;
    }
}
