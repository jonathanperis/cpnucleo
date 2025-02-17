namespace Application.UseCases.Project.ListProject;

public sealed record ListProjectQueryViewModel(OperationResult OperationResult, List<ProjectDto?>? Projects);
