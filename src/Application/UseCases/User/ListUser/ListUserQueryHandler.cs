namespace Application.UseCases.User.ListUser;

public sealed class ListUserQueryHandler(IUserRepository userRepository) : IRequestHandler<ListUserQuery, ListUserQueryViewModel>
{
    public async ValueTask<ListUserQueryViewModel> Handle(ListUserQuery request, CancellationToken cancellationToken)
    {
        var users = await userRepository.ListUsers();

        var operationResult = users is not null ? OperationResult.Success : OperationResult.NotFound;
        var usersList = users ?? [];  // Return an empty list if no users are found

        return new ListUserQueryViewModel(operationResult, usersList);
    }
}
