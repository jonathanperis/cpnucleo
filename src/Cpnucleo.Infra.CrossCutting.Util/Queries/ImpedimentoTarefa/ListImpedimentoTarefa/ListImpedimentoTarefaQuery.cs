namespace Cpnucleo.Infra.CrossCutting.Util.Queries.ImpedimentoTarefa.ListImpedimentoTarefa;

[DataContract]
public class ListImpedimentoTarefaQuery
{
    [DataMember(Order = 1)]
    public bool GetDependencies { get; set; }
}