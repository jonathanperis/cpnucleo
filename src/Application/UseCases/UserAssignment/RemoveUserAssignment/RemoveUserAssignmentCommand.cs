namespace Application.UseCases.UserAssignment.RemoveUserAssignment;

public sealed record RemoveUserAssignmentCommand(Ulid Id) : BaseCommand, IRequest<OperationResult>;
