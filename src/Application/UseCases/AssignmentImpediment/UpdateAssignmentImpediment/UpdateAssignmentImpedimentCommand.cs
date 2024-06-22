namespace Application.UseCases.AssignmentImpediment.UpdateAssignmentImpediment;

public sealed record UpdateAssignmentImpedimentCommand(Ulid Id, string Description, Ulid AssignmentId, Ulid ImpedimentId) : BaseCommand, IRequest<OperationResult>;
