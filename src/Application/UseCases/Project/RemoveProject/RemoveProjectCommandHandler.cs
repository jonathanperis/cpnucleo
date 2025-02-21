namespace Application.UseCases.Project.RemoveProject;

// Dapper Repository Basic
public sealed class RemoveProjectCommandHandler(IProjectRepository projectRepository) : IRequestHandler<RemoveProjectCommand, OperationResult>
{
    public async ValueTask<OperationResult> Handle(RemoveProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetProjectById(request.Id);
        
        if (project is null)
        {
            return OperationResult.NotFound;
        }
        
        var success = await projectRepository.RemoveProject(project.Id);
        
        return success ? OperationResult.Success : OperationResult.Failed;
    }
}
