namespace Application.UseCases.Project.ListProject;

public sealed class ListProjectQueryHandler(IProjectRepository projectRepository) : IRequestHandler<ListProjectQuery, ListProjectQueryViewModel>
{
    public async ValueTask<ListProjectQueryViewModel> Handle(ListProjectQuery request, CancellationToken cancellationToken)
    {
        var projects = await projectRepository.ListProjects();

        var operationResult = projects is not null ? OperationResult.Success : OperationResult.NotFound;

        var result = projects?
                                        .Select(x => x?.MapToDto())
                                        .ToList();

        return new ListProjectQueryViewModel(operationResult, result);
    }
}
