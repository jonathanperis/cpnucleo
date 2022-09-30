namespace Cpnucleo.Shared.Commands.Impedimento;

public sealed record RemoveImpedimentoCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;