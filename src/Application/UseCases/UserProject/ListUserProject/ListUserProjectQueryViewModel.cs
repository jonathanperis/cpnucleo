namespace Application.UseCases.UserProject.ListUserProject;

public sealed record ListUserProjectQueryViewModel(OperationResult OperationResult, PaginatedResult<UserProjectDto?> Result);
