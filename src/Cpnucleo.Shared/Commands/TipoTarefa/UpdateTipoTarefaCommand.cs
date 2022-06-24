namespace Cpnucleo.Infra.CrossCutting.Shared.Commands.TipoTarefa;

public record UpdateTipoTarefaCommand(Guid Id, string Nome, string Image) : BaseCommand, IRequest<OperationResult>;