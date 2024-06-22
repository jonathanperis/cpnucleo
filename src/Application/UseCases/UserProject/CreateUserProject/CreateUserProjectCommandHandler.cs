namespace Application.UseCases.UserProject.CreateUserProject;

public sealed class CreateUserProjectCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<CreateUserProjectCommand, OperationResult>
{
    public async ValueTask<OperationResult> Handle(CreateUserProjectCommand request, CancellationToken cancellationToken)
    {
        var userProject = Domain.Entities.UserProject.Create(request.UserId, request.ProjectId, request.Id);

        if (dbContext.UserProjects is not null)
            await dbContext.UserProjects.AddAsync(userProject, cancellationToken);

        var result = await dbContext.SaveChangesAsync(cancellationToken);

        return result ? OperationResult.Success : OperationResult.Failed;
    }
}
