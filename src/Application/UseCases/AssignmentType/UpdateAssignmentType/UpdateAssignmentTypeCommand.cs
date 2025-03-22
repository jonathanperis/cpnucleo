namespace Application.UseCases.AssignmentType.UpdateAssignmentType;

public sealed record UpdateAssignmentTypeCommand(Guid Id, string Name) : BaseCommand, IRequest<OperationResult>;
