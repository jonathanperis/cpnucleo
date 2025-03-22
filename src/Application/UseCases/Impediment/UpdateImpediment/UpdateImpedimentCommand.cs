namespace Application.UseCases.Impediment.UpdateImpediment;

public sealed record UpdateImpedimentCommand(Guid Id, string Name) : BaseCommand, IRequest<OperationResult>;
