namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso.ListRecurso;

public class ListRecursoResponse : BaseQuery
{
    public OperationResult Status { get; set; }

    public IEnumerable<RecursoViewModel> Recursos { get; set; }
}