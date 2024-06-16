namespace Application.UseCases.Project.RemoveProject;

public sealed class RemoveProjectCommandHandler : IRequestHandler<RemoveProjectCommand, OperationResult>
{
    private readonly IApplicationDbContext _dbContext;

    public RemoveProjectCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<OperationResult> Handle(RemoveProjectCommand request, CancellationToken cancellationToken)
    {
        if (_dbContext.Projects is not null)
        {
            var project = await _dbContext.Projects
                .FirstOrDefaultAsync(p => p.Id == request.Id && p.Active, cancellationToken);

            if (project == null)
            {
                return OperationResult.NotFound;
            }
        }

        var result = await _dbContext.SaveChangesAsync(cancellationToken);

        return result ? OperationResult.Success : OperationResult.Failed;
    }
}
