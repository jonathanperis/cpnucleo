namespace Cpnucleo.Shared.Commands.CreateImpedimento;

public sealed record CreateImpedimentoCommand(string Nome, Guid Id = default) : BaseCommand, IRequest<OperationResult>;