namespace Cpnucleo.Application.Queries.TipoTarefa.ListTipoTarefa;

public class ListTipoTarefaViewModel
{
    public IEnumerable<TipoTarefaDTO> TipoTarefas { get; set; }
    public OperationResult OperationResult { get; set; }
}
