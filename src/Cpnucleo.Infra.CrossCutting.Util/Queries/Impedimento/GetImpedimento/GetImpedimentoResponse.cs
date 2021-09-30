namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Impedimento.GetImpedimento;

[DataContract]
public class GetImpedimentoResponse
{
    [DataMember(Order = 1)]
    public OperationResult Status { get; set; }

    [DataMember(Order = 2)]
    public ImpedimentoViewModel Impedimento { get; set; }
}