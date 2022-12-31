namespace Cpnucleo.Application.Queries.RecursoProjeto;

public sealed class GetRecursoProjetoByProjetoQueryValidator : AbstractValidator<ListRecursoProjetoByProjetoQuery>
{
    public GetRecursoProjetoByProjetoQueryValidator()
    {
        RuleFor(x => x.IdProjeto).NotEmpty();
    }
}
