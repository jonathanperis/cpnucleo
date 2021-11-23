namespace Cpnucleo.Infra.CrossCutting.Util.Queries.TipoTarefa.ListTipoTarefa;

public class ListTipoTarefaResponse : BaseQuery
{
    public OperationResult Status { get; set; }

    public IEnumerable<TipoTarefaViewModel> TipoTarefas { get; set; }
}