namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Sistema;

public class ListSistemaViewModel
{
    public IEnumerable<SistemaDTO> Sistemas { get; set; }
    public OperationResult OperationResult { get; set; }
}
