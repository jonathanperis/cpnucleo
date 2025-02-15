namespace Application.UseCases.Project.RemoveProject;

public sealed class RemoveProjectCommandHandler(ApplicationDbContext dbContext) : IRequestHandler<RemoveProjectCommand, OperationResult>
{
    public async ValueTask<OperationResult> Handle(RemoveProjectCommand request, CancellationToken cancellationToken)
    {
        if (dbContext.Projects is not null)
        {
            var project = await dbContext.Projects
                .FirstOrDefaultAsync(p => p.Id == request.Id && p.Active, cancellationToken);

            if (project is null)
            {
                return OperationResult.NotFound;
            }

            Domain.Entities.Project.Remove(project);
        }

        var result = await dbContext.SaveChangesAsync(cancellationToken);

        return result ? OperationResult.Success : OperationResult.Failed;
    }
}
