namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento.RemoveImpedimento;

[DataContract]
public class RemoveImpedimentoResponse
{
    [DataMember(Order = 1)]
    public OperationResult Status { get; set; }
}