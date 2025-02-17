namespace Application.UseCases.User.ListUser;

public sealed class ListUserQueryHandler(IUserRepository userRepository) : IRequestHandler<ListUserQuery, ListUserQueryViewModel>
{
    public async ValueTask<ListUserQueryViewModel> Handle(ListUserQuery request, CancellationToken cancellationToken)
    {
        var users = await userRepository.ListUsers();

        var operationResult = users is not null ? OperationResult.Success : OperationResult.NotFound;

        var result = users?
                                    .Select(x => x?.MapToDto())
                                    .ToList();

        return new ListUserQueryViewModel(operationResult, result);
    }
}
