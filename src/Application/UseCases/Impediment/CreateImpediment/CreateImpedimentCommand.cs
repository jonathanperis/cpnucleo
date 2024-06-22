namespace Application.UseCases.Impediment.CreateImpediment;

public sealed record CreateImpedimentCommand(string Name, Ulid Id = default) : BaseCommand, IRequest<OperationResult>;
