namespace Cpnucleo.Infra.CrossCutting.Util.Queries.TipoTarefa;

public class GetTipoTarefaViewModel : BaseQuery
{
    public TipoTarefaDTO TipoTarefa { get; set; }
    public OperationResult OperationResult { get; set; }
}
