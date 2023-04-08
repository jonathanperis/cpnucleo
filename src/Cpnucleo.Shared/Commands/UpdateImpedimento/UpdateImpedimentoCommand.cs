namespace Cpnucleo.Shared.Commands.UpdateImpedimento;

public sealed record UpdateImpedimentoCommand(Guid Id, string Nome) : BaseCommand, IRequest<OperationResult>;