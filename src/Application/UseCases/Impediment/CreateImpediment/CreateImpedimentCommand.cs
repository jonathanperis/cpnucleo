namespace Application.UseCases.Impediment.CreateImpediment;

public sealed record CreateImpedimentCommand(string Name, Guid Id = default) : BaseCommand, IRequest<OperationResult>;
