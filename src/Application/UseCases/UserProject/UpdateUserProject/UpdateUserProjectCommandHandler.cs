namespace Application.UseCases.UserProject.UpdateUserProject;

// EF Core
public sealed class UpdateUserProjectCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<UpdateUserProjectCommand, OperationResult>
{
    public async ValueTask<OperationResult> Handle(UpdateUserProjectCommand request, CancellationToken cancellationToken)
    {
        if (dbContext.UserProjects is not null)
        {
            var userProject = await dbContext.UserProjects
                .FirstOrDefaultAsync(p => p.Id == request.Id && p.Active, cancellationToken);

            if (userProject is null)
            {
                return OperationResult.NotFound;
            }

            Domain.Entities.UserProject.Update(userProject, request.UserId, request.ProjectId);
        }

        var result = await dbContext.SaveChangesAsync(cancellationToken);

        return result ? OperationResult.Success : OperationResult.Failed;
    }
}
