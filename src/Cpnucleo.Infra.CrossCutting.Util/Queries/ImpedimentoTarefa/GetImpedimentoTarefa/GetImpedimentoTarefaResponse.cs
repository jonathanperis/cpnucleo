namespace Cpnucleo.Infra.CrossCutting.Util.Queries.ImpedimentoTarefa.GetImpedimentoTarefa;

[DataContract]
public class GetImpedimentoTarefaResponse
{
    [DataMember(Order = 1)]
    public OperationResult Status { get; set; }

    [DataMember(Order = 2)]
    public ImpedimentoTarefaViewModel ImpedimentoTarefa { get; set; }
}