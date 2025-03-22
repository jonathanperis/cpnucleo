namespace Application.UseCases.AssignmentType.RemoveAssignmentType;

public sealed record RemoveAssignmentTypeCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;
