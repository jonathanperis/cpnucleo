namespace Application.UseCases.AssignmentImpediment.RemoveAssignmentImpediment;

public sealed record RemoveAssignmentImpedimentCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;
