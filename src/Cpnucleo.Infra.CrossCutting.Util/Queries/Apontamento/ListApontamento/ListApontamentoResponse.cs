namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento.ListApontamento;

[DataContract]
public class ListApontamentoResponse
{
    [DataMember(Order = 1)]
    public OperationResult Status { get; set; }

    [DataMember(Order = 2)]
    public IEnumerable<ApontamentoViewModel> Apontamentos { get; set; }
}