namespace Application.UseCases.Project.GetProjectById;

public sealed class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, GetProjectByIdQueryViewModel>
{
    private readonly IProjectRepository _projectRepository;

    public GetProjectByIdQueryHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async ValueTask<GetProjectByIdQueryViewModel> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetProjectById(request.Id);

        var operationResult = project is not null ? OperationResult.Success : OperationResult.NotFound;

        return new GetProjectByIdQueryViewModel(operationResult, project);
    }
}
