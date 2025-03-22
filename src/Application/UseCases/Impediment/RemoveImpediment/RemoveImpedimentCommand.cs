namespace Application.UseCases.Impediment.RemoveImpediment;

public sealed record RemoveImpedimentCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;
