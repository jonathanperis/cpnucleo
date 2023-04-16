namespace Cpnucleo.Shared.Queries.GetApontamentoByRecurso;

public sealed class ListApontamentoByRecursoQueryValidator : AbstractValidator<ListApontamentoByRecursoQuery>
{
    public ListApontamentoByRecursoQueryValidator()
    {
        RuleFor(x => x.IdRecurso)
            .NotEmpty()
            .WithMessage("Apontamento deve conter um Recurso");
    }
}
