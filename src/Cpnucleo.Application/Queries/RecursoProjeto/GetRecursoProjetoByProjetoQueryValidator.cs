namespace Cpnucleo.Application.Queries.RecursoProjeto;

public sealed class GetRecursoProjetoByProjetoQueryValidator : AbstractValidator<GetRecursoProjetoByProjetoQuery>
{
    public GetRecursoProjetoByProjetoQueryValidator()
    {
        RuleFor(x => x.IdProjeto).NotEmpty();
    }
}
