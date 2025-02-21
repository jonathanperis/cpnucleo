namespace Application.UseCases.Project.UpdateProject;

// Dapper Repository Basic
public sealed class UpdateProjectCommandHandler(IProjectRepository projectRepository) : IRequestHandler<UpdateProjectCommand, OperationResult>
{
    public async ValueTask<OperationResult> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetProjectById(request.Id);
        
        if (project is null)
        {
            return OperationResult.NotFound;
        }
        
        var success = await projectRepository.UpdateProject(request.Id, request.Name, request.OrganizationId);
        
        return success ? OperationResult.Success : OperationResult.Failed;
    }
}
