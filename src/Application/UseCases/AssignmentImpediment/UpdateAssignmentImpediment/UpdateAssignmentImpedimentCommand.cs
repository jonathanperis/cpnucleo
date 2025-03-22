namespace Application.UseCases.AssignmentImpediment.UpdateAssignmentImpediment;

public sealed record UpdateAssignmentImpedimentCommand(Guid Id, string Description, Guid AssignmentId, Guid ImpedimentId) : BaseCommand, IRequest<OperationResult>;
