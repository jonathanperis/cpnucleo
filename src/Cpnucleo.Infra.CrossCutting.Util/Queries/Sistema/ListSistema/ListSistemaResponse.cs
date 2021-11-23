namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Sistema.ListSistema;

public class ListSistemaResponse : BaseQuery
{
    public OperationResult Status { get; set; }

    public IEnumerable<SistemaViewModel> Sistemas { get; set; }
}