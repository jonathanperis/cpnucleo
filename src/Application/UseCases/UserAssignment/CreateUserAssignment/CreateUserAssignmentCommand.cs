namespace Application.UseCases.UserAssignment.CreateUserAssignment;

public sealed record CreateUserAssignmentCommand(Ulid UserId, Ulid AssignmentId, Ulid Id = default) : BaseCommand, IRequest<OperationResult>;
