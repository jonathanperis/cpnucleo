namespace Application.UseCases.Project.ListProject;

public sealed record ListProjectQueryViewModel(OperationResult OperationResult, PaginatedResult<ProjectDto?> Result);
