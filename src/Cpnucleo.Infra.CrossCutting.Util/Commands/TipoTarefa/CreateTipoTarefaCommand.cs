namespace Cpnucleo.Infra.CrossCutting.Util.Commands.TipoTarefa;

public class CreateTipoTarefaCommand : BaseCommand, IRequest<OperationResult>
{
    public TipoTarefaViewModel TipoTarefa { get; set; }
}