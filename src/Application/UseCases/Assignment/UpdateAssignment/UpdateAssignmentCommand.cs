namespace Application.UseCases.Assignment.UpdateAssignment;

public sealed record UpdateAssignmentCommand(Guid Id, string Name, string Description, DateTime StartDate, DateTime EndDate, byte AmountHours, Guid ProjectId, Guid WorkflowId, Guid UserId, Guid AssignmentTypeId) : BaseCommand, IRequest<OperationResult>;
