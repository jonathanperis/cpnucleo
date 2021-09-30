namespace Cpnucleo.Infra.CrossCutting.Util.Commands.ImpedimentoTarefa.UpdateImpedimentoTarefa;

[DataContract]
public class UpdateImpedimentoTarefaResponse
{
    [DataMember(Order = 1)]
    public OperationResult Status { get; set; }
}