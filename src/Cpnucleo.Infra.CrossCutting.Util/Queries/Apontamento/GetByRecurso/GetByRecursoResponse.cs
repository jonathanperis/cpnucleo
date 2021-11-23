namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento.GetByRecurso;

public class GetByRecursoResponse : BaseQuery
{
    public OperationResult Status { get; set; }

    public IEnumerable<ApontamentoViewModel> Apontamentos { get; set; }
}