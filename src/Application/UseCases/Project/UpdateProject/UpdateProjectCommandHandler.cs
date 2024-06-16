namespace Application.UseCases.Project.UpdateProject;

public sealed class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, OperationResult>
{
    private readonly IApplicationDbContext _dbContext;

    public UpdateProjectCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<OperationResult> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        if (_dbContext.Projects is not null)
        {
            var project = await _dbContext.Projects
                .FirstOrDefaultAsync(p => p.Id == request.Id && p.Active, cancellationToken);

            if (project == null)
            {
                return OperationResult.NotFound;
            }

            project = Domain.Entities.Project.Update(project, request.Name, request.SystemId);
        }

        var result = await _dbContext.SaveChangesAsync(cancellationToken);

        return result ? OperationResult.Success : OperationResult.Failed;
    }
}
