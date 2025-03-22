namespace Application.UseCases.UserAssignment.CreateUserAssignment;

public sealed record CreateUserAssignmentCommand(Guid UserId, Guid AssignmentId, Guid Id = default) : BaseCommand, IRequest<OperationResult>;
