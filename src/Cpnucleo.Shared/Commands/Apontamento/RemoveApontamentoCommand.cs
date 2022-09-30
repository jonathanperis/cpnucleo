namespace Cpnucleo.Shared.Commands.Apontamento;

public sealed record RemoveApontamentoCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;