namespace Cpnucleo.Infra.CrossCutting.Util.Queries.ImpedimentoTarefa.ListImpedimentoTarefa;

[DataContract]
public class ListImpedimentoTarefaResponse
{
    [DataMember(Order = 1)]
    public OperationResult Status { get; set; }

    [DataMember(Order = 2)]
    public IEnumerable<ImpedimentoTarefaViewModel> ImpedimentoTarefas { get; set; }
}