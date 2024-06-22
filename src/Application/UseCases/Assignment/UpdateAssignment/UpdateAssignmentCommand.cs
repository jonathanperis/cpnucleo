namespace Application.UseCases.Assignment.UpdateAssignment;

public sealed record UpdateAssignmentCommand(Ulid Id, string Name, string Description, DateTime StartDate, DateTime EndDate, byte AmountHours, Ulid ProjectId, Ulid WorkflowId, Ulid UserId, Ulid AssignmentTypeId) : BaseCommand, IRequest<OperationResult>;
