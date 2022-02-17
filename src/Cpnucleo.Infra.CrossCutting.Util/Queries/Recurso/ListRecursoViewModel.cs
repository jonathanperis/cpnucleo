namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso;

public class ListRecursoViewModel
{
    public IEnumerable<RecursoDTO> Recursos { get; set; }
    public OperationResult OperationResult { get; set; }
}
