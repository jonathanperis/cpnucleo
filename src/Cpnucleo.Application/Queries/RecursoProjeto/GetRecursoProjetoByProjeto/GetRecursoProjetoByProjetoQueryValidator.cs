namespace Cpnucleo.Application.Queries.RecursoProjeto.GetRecursoProjetoByProjeto;

public class GetRecursoProjetoByProjetoQueryValidator : AbstractValidator<GetRecursoProjetoByProjetoQuery>
{
    public GetRecursoProjetoByProjetoQueryValidator()
    {
        RuleFor(x => x.IdProjeto).NotEmpty();
    }
}
