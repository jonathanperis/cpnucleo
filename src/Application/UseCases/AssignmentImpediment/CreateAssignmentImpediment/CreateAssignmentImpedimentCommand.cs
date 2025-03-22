namespace Application.UseCases.AssignmentImpediment.CreateAssignmentImpediment;

public sealed record CreateAssignmentImpedimentCommand(string Description, Guid AssignmentId, Guid ImpedimentId, Guid Id = default) : BaseCommand, IRequest<OperationResult>;
