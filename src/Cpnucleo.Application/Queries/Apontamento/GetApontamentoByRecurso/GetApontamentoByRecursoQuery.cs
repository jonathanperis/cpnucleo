namespace Cpnucleo.Application.Queries.Apontamento.GetApontamentoByRecurso;

public class GetApontamentoByRecursoQuery : IRequest<GetApontamentoByRecursoViewModel>
{
    public Guid IdRecurso { get; set; }
}
