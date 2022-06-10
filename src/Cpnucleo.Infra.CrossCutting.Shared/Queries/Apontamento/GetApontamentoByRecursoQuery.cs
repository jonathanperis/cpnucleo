namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.Apontamento;

public class GetApontamentoByRecursoQuery : BaseQuery, IRequest<GetApontamentoByRecursoViewModel>
{
    public Guid IdRecurso { get; set; }
}
