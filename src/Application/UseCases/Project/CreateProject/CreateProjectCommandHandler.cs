namespace Application.UseCases.Project.CreateProject;

public sealed class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, OperationResult>
{
    private readonly IApplicationDbContext _dbContext;

    public CreateProjectCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<OperationResult> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = Domain.Entities.Project.Create(request.Name, request.Id);

        if (_dbContext.Projects is not null)
            await _dbContext.Projects.AddAsync(project, cancellationToken);

        var result = await _dbContext.SaveChangesAsync(cancellationToken);

        return result ? OperationResult.Success : OperationResult.Failed;
    }
}
