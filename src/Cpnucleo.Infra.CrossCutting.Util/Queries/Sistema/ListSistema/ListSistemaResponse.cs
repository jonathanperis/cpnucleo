namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Sistema.ListSistema;

[DataContract]
public class ListSistemaResponse
{
    [DataMember(Order = 1)]
    public OperationResult Status { get; set; }

    [DataMember(Order = 2)]
    public IEnumerable<SistemaViewModel> Sistemas { get; set; }
}