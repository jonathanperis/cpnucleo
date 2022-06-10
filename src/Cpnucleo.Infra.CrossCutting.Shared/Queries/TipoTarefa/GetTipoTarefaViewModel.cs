namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.TipoTarefa;

public class GetTipoTarefaViewModel : BaseQuery
{
    public TipoTarefaDTO TipoTarefa { get; set; }
    public OperationResult OperationResult { get; set; }
}
