namespace Application.UseCases.Project.GetProjectById;

public sealed class GetProjectByIdQueryHandler(IProjectRepository projectRepository) : IRequestHandler<GetProjectByIdQuery, GetProjectByIdQueryViewModel>
{
    public async ValueTask<GetProjectByIdQueryViewModel> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetProjectById(request.Id);

        var operationResult = project is not null ? OperationResult.Success : OperationResult.NotFound;

        return new GetProjectByIdQueryViewModel(operationResult, project);
    }
}
