namespace Cpnucleo.Infra.CrossCutting.Util.Queries.TipoTarefa.GetTipoTarefa;

public class GetTipoTarefaResponse : BaseQuery
{
    public OperationResult Status { get; set; }

    public TipoTarefaViewModel TipoTarefa { get; set; }
}