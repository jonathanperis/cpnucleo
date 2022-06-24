namespace Cpnucleo.Infra.CrossCutting.Shared.Commands.Apontamento;

public record RemoveApontamentoCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;