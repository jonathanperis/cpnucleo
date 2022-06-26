namespace Cpnucleo.Shared.Commands.Impedimento;

public record CreateImpedimentoCommand(Guid Id, string Nome) : BaseCommand, IRequest<OperationResult>;