namespace Application.UseCases.Project.CreateProject;

public sealed class CreateProjectCommandHandler(ApplicationDbContext dbContext) : IRequestHandler<CreateProjectCommand, OperationResult>
{
    public async ValueTask<OperationResult> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = Domain.Entities.Project.Create(request.Name, request.Id);

        if (dbContext.Projects is not null)
            await dbContext.Projects.AddAsync(project, cancellationToken);

        var result = await dbContext.SaveChangesAsync(cancellationToken);

        return result ? OperationResult.Success : OperationResult.Failed;
    }
}
