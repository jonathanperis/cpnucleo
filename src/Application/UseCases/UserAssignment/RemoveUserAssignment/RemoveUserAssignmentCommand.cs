namespace Application.UseCases.UserAssignment.RemoveUserAssignment;

public sealed record RemoveUserAssignmentCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;
