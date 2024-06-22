namespace Application.UseCases.Assignment.RemoveAssignment;

public sealed record RemoveAssignmentCommand(Ulid Id) : BaseCommand, IRequest<OperationResult>;
