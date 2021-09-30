namespace Cpnucleo.Infra.CrossCutting.Util.Queries.TipoTarefa.ListTipoTarefa;

[DataContract]
public class ListTipoTarefaResponse
{
    [DataMember(Order = 1)]
    public OperationResult Status { get; set; }

    [DataMember(Order = 2)]
    public IEnumerable<TipoTarefaViewModel> TipoTarefas { get; set; }
}