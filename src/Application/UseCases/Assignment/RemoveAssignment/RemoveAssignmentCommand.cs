namespace Application.UseCases.Assignment.RemoveAssignment;

public sealed record RemoveAssignmentCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;
