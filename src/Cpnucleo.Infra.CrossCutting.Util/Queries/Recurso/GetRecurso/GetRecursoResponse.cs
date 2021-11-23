namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso.GetRecurso;

public class GetRecursoResponse : BaseQuery
{
    public OperationResult Status { get; set; }

    public RecursoViewModel Recurso { get; set; }
}