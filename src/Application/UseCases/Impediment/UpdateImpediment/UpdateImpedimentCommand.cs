namespace Application.UseCases.Impediment.UpdateImpediment;

public sealed record UpdateImpedimentCommand(Ulid Id, string Name) : BaseCommand, IRequest<OperationResult>;
