namespace Cpnucleo.Infra.CrossCutting.Util.Commands.TipoTarefa;

public class UpdateTipoTarefaCommand : BaseCommand, IRequest<OperationResult>
{
    public TipoTarefaViewModel TipoTarefa { get; set; }
}