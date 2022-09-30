namespace Cpnucleo.Shared.Commands.Impedimento;

public sealed record UpdateImpedimentoCommand(Guid Id, string Nome) : BaseCommand, IRequest<OperationResult>;