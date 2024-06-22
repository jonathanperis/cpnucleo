namespace Application.UseCases.UserProject.GetUserProjectById;

public sealed class GetUserProjectByIdQueryHandler(IUserProjectRepository userProjectRepository) : IRequestHandler<GetUserProjectByIdQuery, GetUserProjectByIdQueryViewModel>
{
    public async ValueTask<GetUserProjectByIdQueryViewModel> Handle(GetUserProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var userProject = await userProjectRepository.GetUserProjectById(request.Id);

        var operationResult = userProject is not null ? OperationResult.Success : OperationResult.NotFound;

        return new GetUserProjectByIdQueryViewModel(operationResult, userProject);
    }
}
