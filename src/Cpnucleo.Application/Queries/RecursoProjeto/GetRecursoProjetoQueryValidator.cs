using Cpnucleo.Shared.Queries.RecursoProjeto;

namespace Cpnucleo.Application.Queries.RecursoProjeto;

public class GetRecursoProjetoQueryValidator : AbstractValidator<GetRecursoProjetoQuery>
{
    public GetRecursoProjetoQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
