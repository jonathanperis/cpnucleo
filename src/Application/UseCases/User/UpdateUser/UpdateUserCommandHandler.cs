namespace Application.UseCases.User.UpdateUser;

// EF Core
public sealed class UpdateUserCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<UpdateUserCommand, OperationResult>
{
    public async ValueTask<OperationResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        if (dbContext.Users is not null)
        {
            var user = await dbContext.Users
                .FirstOrDefaultAsync(p => p.Id == request.Id && p.Active, cancellationToken);

            if (user is null)
            {
                return OperationResult.NotFound;
            }

            Domain.Entities.User.Update(user, request.Name, request.Password);
        }

        var result = await dbContext.SaveChangesAsync(cancellationToken);

        return result ? OperationResult.Success : OperationResult.Failed;
    }
}
