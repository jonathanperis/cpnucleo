namespace Cpnucleo.Application.Queries.Sistema.ListSistema;

public class ListSistemaViewModel
{
    public IEnumerable<SistemaDTO> Sistemas { get; set; }
    public OperationResult OperationResult { get; set; }
}
