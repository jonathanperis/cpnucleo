namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa.GetTarefa;

public class GetTarefaResponse : BaseQuery
{
    public OperationResult Status { get; set; }

    public TarefaViewModel Tarefa { get; set; }
}