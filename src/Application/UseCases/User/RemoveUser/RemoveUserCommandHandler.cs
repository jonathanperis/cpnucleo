namespace Application.UseCases.User.RemoveUser;

// EF Core
public sealed class RemoveUserCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<RemoveUserCommand, OperationResult>
{
    public async ValueTask<OperationResult> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
    {
        if (dbContext.Users is not null)
        {
            var user = await dbContext.Users
                .FirstOrDefaultAsync(p => p.Id == request.Id && p.Active, cancellationToken);

            if (user is null)
            {
                return OperationResult.NotFound;
            }

            Domain.Entities.User.Remove(user);
        }

        var result = await dbContext.SaveChangesAsync(cancellationToken);

        return result ? OperationResult.Success : OperationResult.Failed;
    }
}
