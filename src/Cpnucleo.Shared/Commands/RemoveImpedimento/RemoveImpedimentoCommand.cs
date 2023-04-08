namespace Cpnucleo.Shared.Commands.RemoveImpedimento;

public sealed record RemoveImpedimentoCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;