namespace Application.UseCases.UserProject.RemoveUserProject;

public sealed class RemoveUserProjectCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<RemoveUserProjectCommand, OperationResult>
{
    public async ValueTask<OperationResult> Handle(RemoveUserProjectCommand request, CancellationToken cancellationToken)
    {
        if (dbContext.UserProjects is not null)
        {
            var userProject = await dbContext.UserProjects
                .FirstOrDefaultAsync(p => p.Id == request.Id && p.Active, cancellationToken);

            if (userProject is null)
            {
                return OperationResult.NotFound;
            }

            Domain.Entities.UserProject.Remove(userProject);
        }

        var result = await dbContext.SaveChangesAsync(cancellationToken);

        return result ? OperationResult.Success : OperationResult.Failed;
    }
}
