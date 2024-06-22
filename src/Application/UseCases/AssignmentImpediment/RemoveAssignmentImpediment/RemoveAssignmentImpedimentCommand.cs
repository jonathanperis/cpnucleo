namespace Application.UseCases.AssignmentImpediment.RemoveAssignmentImpediment;

public sealed record RemoveAssignmentImpedimentCommand(Ulid Id) : BaseCommand, IRequest<OperationResult>;
