using Cpnucleo.Shared.Queries.RecursoProjeto;

namespace Cpnucleo.Application.Queries.RecursoProjeto;

public class GetRecursoProjetoByProjetoQueryValidator : AbstractValidator<GetRecursoProjetoByProjetoQuery>
{
    public GetRecursoProjetoByProjetoQueryValidator()
    {
        RuleFor(x => x.IdProjeto).NotEmpty();
    }
}
