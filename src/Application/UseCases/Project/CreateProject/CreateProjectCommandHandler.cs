namespace Application.UseCases.Project.CreateProject;

// Dapper Repository Basic
public sealed class CreateProjectCommandHandler(IProjectRepository projectRepository) : IRequestHandler<CreateProjectCommand, OperationResult>
{
    public async ValueTask<OperationResult> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = Domain.Entities.Project.Create(request.Name, request.Id);

        var result = await projectRepository.CreateProject(project);

        return result ? OperationResult.Success : OperationResult.Failed;
    }
}
