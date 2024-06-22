namespace Application.UseCases.AssignmentType.UpdateAssignmentType;

public sealed record UpdateAssignmentTypeCommand(Ulid Id, string Name) : BaseCommand, IRequest<OperationResult>;
