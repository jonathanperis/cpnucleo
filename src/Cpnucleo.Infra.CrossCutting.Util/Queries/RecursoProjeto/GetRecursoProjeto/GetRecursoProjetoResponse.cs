namespace Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto.GetRecursoProjeto;

public class GetRecursoProjetoResponse : BaseQuery
{
    public OperationResult Status { get; set; }

    public RecursoProjetoViewModel RecursoProjeto { get; set; }
}