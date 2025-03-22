namespace Application.UseCases.User.GetUserById;

public sealed record GetUserByIdQueryViewModel(OperationResult OperationResult, UserDto? User);
