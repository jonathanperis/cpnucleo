namespace Application.UseCases.Project.UpdateProject;

public sealed class UpdateProjectCommandHandler(ApplicationDbContext dbContext) : IRequestHandler<UpdateProjectCommand, OperationResult>
{
    public async ValueTask<OperationResult> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        if (dbContext.Projects is not null)
        {
            var project = await dbContext.Projects
                .FirstOrDefaultAsync(p => p.Id == request.Id && p.Active, cancellationToken);

            if (project is null)
            {
                return OperationResult.NotFound;
            }

            Domain.Entities.Project.Update(project, request.Name, request.OrganizationId);
        }

        var result = await dbContext.SaveChangesAsync(cancellationToken);

        return result ? OperationResult.Success : OperationResult.Failed;
    }
}
