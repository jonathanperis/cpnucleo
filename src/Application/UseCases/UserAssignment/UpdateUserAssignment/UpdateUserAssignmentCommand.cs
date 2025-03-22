namespace Application.UseCases.UserAssignment.UpdateUserAssignment;

public sealed record UpdateUserAssignmentCommand(Guid Id, Guid UserId, Guid AssignmentId) : BaseCommand, IRequest<OperationResult>;
