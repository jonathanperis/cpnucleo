namespace Application.UseCases.Project.UpdateProject;

// Dapper Repository Basic
public sealed class UpdateProjectCommandHandler(IProjectRepository repository) : IRequestHandler<UpdateProjectCommand, OperationResult>
{
    public async ValueTask<OperationResult> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await repository.GetByIdAsync(request.Id);

        if (project is null)
        {
            return OperationResult.NotFound;
        }
            
        Domain.Entities.Project.Update(project, request.Name, project.OrganizationId);
        var success = await repository.UpdateAsync(project);    
        
        return success ? OperationResult.Success : OperationResult.Failed;
    }
}