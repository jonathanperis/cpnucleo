namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento;

public class GetApontamentoByRecursoQuery : IRequest<GetApontamentoByRecursoViewModel>
{
    public Guid IdRecurso { get; set; }
}
