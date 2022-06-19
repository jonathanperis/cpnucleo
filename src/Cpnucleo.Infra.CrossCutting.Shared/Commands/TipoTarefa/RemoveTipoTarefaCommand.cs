namespace Cpnucleo.Infra.CrossCutting.Shared.Commands.TipoTarefa;

public record RemoveTipoTarefaCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;