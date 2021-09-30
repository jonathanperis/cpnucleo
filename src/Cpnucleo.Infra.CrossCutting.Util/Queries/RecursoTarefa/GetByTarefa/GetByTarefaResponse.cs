namespace Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa.GetByTarefa;

[DataContract]
public class GetByTarefaResponse
{
    [DataMember(Order = 1)]
    public OperationResult Status { get; set; }

    [DataMember(Order = 2)]
    public IEnumerable<RecursoTarefaViewModel> RecursoTarefas { get; set; }
}