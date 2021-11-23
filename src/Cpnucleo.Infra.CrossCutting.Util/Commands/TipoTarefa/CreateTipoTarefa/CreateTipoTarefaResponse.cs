namespace Cpnucleo.Infra.CrossCutting.Util.Commands.TipoTarefa.CreateTipoTarefa;

public class CreateTipoTarefaResponse : BaseCommand
{
    public OperationResult Status { get; set; }

    public TipoTarefaViewModel TipoTarefa { get; set; }
}