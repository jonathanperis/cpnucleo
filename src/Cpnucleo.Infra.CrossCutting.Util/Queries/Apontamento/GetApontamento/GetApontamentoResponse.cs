namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento.GetApontamento;

[DataContract]
public class GetApontamentoResponse
{
    [DataMember(Order = 1)]
    public OperationResult Status { get; set; }

    [DataMember(Order = 2)]
    public ApontamentoViewModel Apontamento { get; set; }
}