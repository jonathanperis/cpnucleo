namespace Application.UseCases.Project.ListProject;

public sealed class ListProjectQueryHandler(IProjectRepository projectRepository) : IRequestHandler<ListProjectQuery, ListProjectQueryViewModel>
{
    public async ValueTask<ListProjectQueryViewModel> Handle(ListProjectQuery request, CancellationToken cancellationToken)
    {
        var projects = await projectRepository.ListProjects();

        var operationResult = projects is not null ? OperationResult.Success : OperationResult.NotFound;
        var projectsList = projects ?? [];  // Return an empty list if no projects are found

        var result = projectsList.Select(project => (ProjectDto)project).ToList();

        return new ListProjectQueryViewModel(operationResult, result);
    }
}
