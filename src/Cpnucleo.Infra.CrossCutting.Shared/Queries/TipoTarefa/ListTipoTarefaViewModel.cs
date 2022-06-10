namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.TipoTarefa;

public class ListTipoTarefaViewModel : BaseQuery
{
    public IEnumerable<TipoTarefaDTO> TipoTarefas { get; set; }
    public OperationResult OperationResult { get; set; }
}
