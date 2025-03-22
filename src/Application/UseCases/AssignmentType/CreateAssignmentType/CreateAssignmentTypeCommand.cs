namespace Application.UseCases.AssignmentType.CreateAssignmentType;

public sealed record CreateAssignmentTypeCommand(string Name, Guid Id = default) : BaseCommand, IRequest<OperationResult>;
