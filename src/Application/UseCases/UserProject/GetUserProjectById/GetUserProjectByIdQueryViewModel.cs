namespace Application.UseCases.UserProject.GetUserProjectById;

public sealed record GetUserProjectByIdQueryViewModel(OperationResult OperationResult, UserProjectDto? UserProject);
