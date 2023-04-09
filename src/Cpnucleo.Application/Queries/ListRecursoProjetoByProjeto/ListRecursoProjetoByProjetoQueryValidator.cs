namespace Cpnucleo.Application.Queries.ListRecursoProjetoByProjeto;

public sealed class ListRecursoProjetoByProjetoQueryValidator : AbstractValidator<ListRecursoProjetoByProjetoQuery>
{
    public ListRecursoProjetoByProjetoQueryValidator()
    {
        RuleFor(x => x.IdProjeto).NotEmpty();
    }
}
