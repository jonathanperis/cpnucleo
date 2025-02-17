namespace Application.UseCases.User.GetUserById;

public sealed class GetUserByIdQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUserByIdQuery, GetUserByIdQueryViewModel>
{
    public async ValueTask<GetUserByIdQueryViewModel> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserById(request.Id);

        var operationResult = user is not null ? OperationResult.Success : OperationResult.NotFound;

        return new GetUserByIdQueryViewModel(operationResult, user?.MapToDto());
    }
}
