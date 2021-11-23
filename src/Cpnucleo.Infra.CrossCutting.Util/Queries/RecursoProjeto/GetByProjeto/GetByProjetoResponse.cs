namespace Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto.GetByProjeto;

public class GetByProjetoResponse : BaseQuery
{
    public OperationResult Status { get; set; }

    public IEnumerable<RecursoProjetoViewModel> RecursoProjetos { get; set; }
}