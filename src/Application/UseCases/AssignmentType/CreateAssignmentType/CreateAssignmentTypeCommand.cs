namespace Application.UseCases.AssignmentType.CreateAssignmentType;

public sealed record CreateAssignmentTypeCommand(string Name, Ulid Id = default) : BaseCommand, IRequest<OperationResult>;
