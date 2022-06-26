namespace Cpnucleo.Shared.Commands.Apontamento;

public record RemoveApontamentoCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;