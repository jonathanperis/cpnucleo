namespace Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa.ListRecursoTarefa;

[DataContract]
public class ListRecursoTarefaResponse
{
    [DataMember(Order = 1)]
    public OperationResult Status { get; set; }

    [DataMember(Order = 2)]
    public IEnumerable<RecursoTarefaViewModel> RecursoTarefas { get; set; }
}