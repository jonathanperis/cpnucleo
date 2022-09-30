namespace Cpnucleo.Shared.Commands.Impedimento;

public sealed record CreateImpedimentoCommand(Guid Id, string Nome) : BaseCommand, IRequest<OperationResult>;