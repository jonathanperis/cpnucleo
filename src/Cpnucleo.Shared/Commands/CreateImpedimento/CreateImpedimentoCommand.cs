namespace Cpnucleo.Shared.Commands.CreateImpedimento;

public sealed record CreateImpedimentoCommand(string Nome) : BaseCommand, IRequest<OperationResult>;