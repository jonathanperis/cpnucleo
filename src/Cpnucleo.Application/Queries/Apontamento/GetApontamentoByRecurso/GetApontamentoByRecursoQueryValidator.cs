namespace Cpnucleo.Application.Queries.Apontamento.GetApontamentoByRecurso;

public class GetApontamentoByRecursoQueryValidator : AbstractValidator<GetApontamentoByRecursoQuery>
{
    public GetApontamentoByRecursoQueryValidator()
    {
        RuleFor(x => x.IdRecurso).NotEmpty();
    }
}
