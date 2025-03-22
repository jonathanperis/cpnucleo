namespace Application.UseCases.Project.RemoveProject;

// Dapper Repository Basic
public sealed class RemoveProjectCommandHandler(IProjectRepository repository) : IRequestHandler<RemoveProjectCommand, OperationResult>
{
    public async ValueTask<OperationResult> Handle(RemoveProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await repository.GetByIdAsync(request.Id);

        if (project is null)
        {
            return OperationResult.NotFound;
        }
            
        Domain.Entities.Project.Remove(project);
        var success = await repository.UpdateAsync(project);    
            
        return success ? OperationResult.Success : OperationResult.Failed;
    }
}