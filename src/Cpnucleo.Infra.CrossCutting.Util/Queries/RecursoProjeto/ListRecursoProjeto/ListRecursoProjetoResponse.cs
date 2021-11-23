namespace Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto.ListRecursoProjeto;

public class ListRecursoProjetoResponse : BaseQuery
{
    public OperationResult Status { get; set; }

    public IEnumerable<RecursoProjetoViewModel> RecursoProjetos { get; set; }
}