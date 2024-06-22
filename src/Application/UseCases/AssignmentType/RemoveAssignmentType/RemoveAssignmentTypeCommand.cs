namespace Application.UseCases.AssignmentType.RemoveAssignmentType;

public sealed record RemoveAssignmentTypeCommand(Ulid Id) : BaseCommand, IRequest<OperationResult>;
