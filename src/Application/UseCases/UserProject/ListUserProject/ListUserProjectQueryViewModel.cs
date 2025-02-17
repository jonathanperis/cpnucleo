namespace Application.UseCases.UserProject.ListUserProject;

public sealed record ListUserProjectQueryViewModel(OperationResult OperationResult, List<UserProjectDto?>? UserProjects);
