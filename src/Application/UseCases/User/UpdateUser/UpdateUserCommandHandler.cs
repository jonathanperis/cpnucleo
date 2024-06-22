namespace Application.UseCases.User.UpdateUser;

public sealed class UpdateUserCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<UpdateUserCommand, OperationResult>
{
    public async ValueTask<OperationResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        if (dbContext.Users is not null)
        {
            var user = await dbContext.Users
                .FirstOrDefaultAsync(p => p.Id == request.Id && p.Active, cancellationToken);

            if (user == null)
            {
                return OperationResult.NotFound;
            }

            user = Domain.Entities.User.Update(user, request.Name, request.Password);
        }

        var result = await dbContext.SaveChangesAsync(cancellationToken);

        return result ? OperationResult.Success : OperationResult.Failed;
    }
}
