using Cpnucleo.Shared.Queries.ListApontamentoByRecurso;

namespace Cpnucleo.Application.Queries.GetApontamentoByRecurso;

public sealed class ListApontamentoByRecursoQueryValidator : AbstractValidator<ListApontamentoByRecursoQuery>
{
    public ListApontamentoByRecursoQueryValidator()
    {
        RuleFor(x => x.IdRecurso).NotEmpty();
    }
}
