namespace Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto;

public class ListRecursoProjetoViewModel : BaseQuery
{
    public IEnumerable<RecursoProjetoDTO> RecursoProjetos { get; set; }
    public OperationResult OperationResult { get; set; }
}
