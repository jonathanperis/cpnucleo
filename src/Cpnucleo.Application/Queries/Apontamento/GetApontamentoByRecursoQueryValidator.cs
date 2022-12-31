namespace Cpnucleo.Application.Queries.Apontamento;

public sealed class GetApontamentoByRecursoQueryValidator : AbstractValidator<ListApontamentoByRecursoQuery>
{
    public GetApontamentoByRecursoQueryValidator()
    {
        RuleFor(x => x.IdRecurso).NotEmpty();
    }
}
