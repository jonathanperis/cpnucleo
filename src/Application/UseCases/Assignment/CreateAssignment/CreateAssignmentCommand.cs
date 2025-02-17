namespace Application.UseCases.Assignment.CreateAssignment;

public sealed record CreateAssignmentCommand(string Name, string Description, DateTime StartDate, DateTime EndDate, byte AmountHours, Guid ProjectId, Guid WorkflowId, Guid UserId, Guid AssignmentTypeId, Guid Id = default) : BaseCommand, IRequest<OperationResult>;
