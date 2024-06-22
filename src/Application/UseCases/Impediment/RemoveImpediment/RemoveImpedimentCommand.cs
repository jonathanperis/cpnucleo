namespace Application.UseCases.Impediment.RemoveImpediment;

public sealed record RemoveImpedimentCommand(Ulid Id) : BaseCommand, IRequest<OperationResult>;
