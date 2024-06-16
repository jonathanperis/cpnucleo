namespace Application.UseCases.Project.ListProject;

public sealed class ListProjectQueryHandler : IRequestHandler<ListProjectQuery, ListProjectQueryViewModel>
{
    private readonly IProjectRepository _projectRepository;

    public ListProjectQueryHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async ValueTask<ListProjectQueryViewModel> Handle(ListProjectQuery request, CancellationToken cancellationToken)
    {
        var projects = await _projectRepository.ListProjects();

        var operationResult = projects is not null ? OperationResult.Success : OperationResult.NotFound;
        var projectsList = projects ?? new List<ProjectDto>();  // Return an empty list if no projects are found

        return new ListProjectQueryViewModel(operationResult, projectsList);
    }
}
