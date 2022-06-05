namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento;

public class GetApontamentoByRecursoQuery : BaseQuery, IRequest<GetApontamentoByRecursoViewModel>
{
    public Guid IdRecurso { get; set; }
}
