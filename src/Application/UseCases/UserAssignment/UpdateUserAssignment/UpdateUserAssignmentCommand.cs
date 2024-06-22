namespace Application.UseCases.UserAssignment.UpdateUserAssignment;

public sealed record UpdateUserAssignmentCommand(Ulid Id, Ulid UserId, Ulid AssignmentId) : BaseCommand, IRequest<OperationResult>;
