namespace Cpnucleo.Application.Queries.Recurso.ListRecurso;

public class ListRecursoViewModel
{
    public IEnumerable<RecursoDTO> Recursos { get; set; }
    public OperationResult OperationResult { get; set; }
}
