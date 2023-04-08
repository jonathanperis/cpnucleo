namespace Cpnucleo.Shared.Commands.CreateImpedimento;

public sealed record CreateImpedimentoCommand(Guid Id, string Nome) : BaseCommand, IRequest<OperationResult>;