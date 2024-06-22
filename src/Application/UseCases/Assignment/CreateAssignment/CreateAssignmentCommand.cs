namespace Application.UseCases.Assignment.CreateAssignment;

public sealed record CreateAssignmentCommand(string Name, string Description, DateTime StartDate, DateTime EndDate, byte AmountHours, Ulid ProjectId, Ulid WorkflowId, Ulid UserId, Ulid AssignmentTypeId, Ulid Id = default) : BaseCommand, IRequest<OperationResult>;
