namespace Cpnucleo.Infra.CrossCutting.Shared.Commands.Impedimento;

public record UpdateImpedimentoCommand(Guid Id, string Nome) : BaseCommand, IRequest<OperationResult>;