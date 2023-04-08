namespace Cpnucleo.Shared.Commands.RemoveApontamento;

public sealed record RemoveApontamentoCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;