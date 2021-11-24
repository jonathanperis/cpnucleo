namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento;

public class GetByRecursoQuery : BaseQuery, IRequest<IEnumerable<ApontamentoViewModel>>
{
    public Guid IdRecurso { get; set; }
}