namespace Application.UseCases.Project.CreateProject;

// Dapper Repository Basic
public sealed class CreateProjectCommandHandler(IProjectRepository repository) : IRequestHandler<CreateProjectCommand, OperationResult>
{
    public async ValueTask<OperationResult> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = Domain.Entities.Project.Create(request.Name, request.OrganizationId, request.Id);
            
        var response = await repository.AddAsync(project);    
            
        return response != Guid.Empty ? OperationResult.Success : OperationResult.Failed;
    }
}