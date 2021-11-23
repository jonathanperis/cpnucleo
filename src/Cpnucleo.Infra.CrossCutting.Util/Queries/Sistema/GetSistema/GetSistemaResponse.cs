namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Sistema.GetSistema;

public class GetSistemaResponse : BaseQuery
{
    public OperationResult Status { get; set; }

    public SistemaViewModel Sistema { get; set; }
}