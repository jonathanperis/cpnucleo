namespace Application.UseCases.Project.GetProjectById;

public sealed record GetProjectByIdQueryViewModel(OperationResult OperationResult, ProjectDto? Project);
