namespace Application.UseCases.AssignmentImpediment.CreateAssignmentImpediment;

public sealed record CreateAssignmentImpedimentCommand(string Description, Ulid AssignmentId, Ulid ImpedimentId, Ulid Id = default) : BaseCommand, IRequest<OperationResult>;
