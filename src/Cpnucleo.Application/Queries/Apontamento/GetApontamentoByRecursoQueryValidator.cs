namespace Cpnucleo.Application.Queries.Apontamento;

public sealed class GetApontamentoByRecursoQueryValidator : AbstractValidator<GetApontamentoByRecursoQuery>
{
    public GetApontamentoByRecursoQueryValidator()
    {
        RuleFor(x => x.IdRecurso).NotEmpty();
    }
}
