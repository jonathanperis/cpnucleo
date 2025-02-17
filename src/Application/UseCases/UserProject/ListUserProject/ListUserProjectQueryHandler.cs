namespace Application.UseCases.UserProject.ListUserProject;

public sealed class ListUserProjectQueryHandler(IUserProjectRepository userProjectRepository) : IRequestHandler<ListUserProjectQuery, ListUserProjectQueryViewModel>
{
    public async ValueTask<ListUserProjectQueryViewModel> Handle(ListUserProjectQuery request, CancellationToken cancellationToken)
    {
        var userProjects = await userProjectRepository.ListUserProjects();

        var operationResult = userProjects is not null ? OperationResult.Success : OperationResult.NotFound;
        var userProjectsList = userProjects ?? [];  // Return an empty list if no user projects are found

        var result = userProjectsList.Select(userProject => (UserProjectDto)userProject).ToList();

        return new ListUserProjectQueryViewModel(operationResult, result);
    }
}
