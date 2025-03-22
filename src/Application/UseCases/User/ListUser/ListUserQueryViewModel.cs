namespace Application.UseCases.User.ListUser;

public sealed record ListUserQueryViewModel(OperationResult OperationResult, PaginatedResult<UserDto?> Result);
