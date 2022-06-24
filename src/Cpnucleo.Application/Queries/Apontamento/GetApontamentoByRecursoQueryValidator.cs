using Cpnucleo.Shared.Queries.Apontamento;

namespace Cpnucleo.Application.Queries.Apontamento;

public class GetApontamentoByRecursoQueryValidator : AbstractValidator<GetApontamentoByRecursoQuery>
{
    public GetApontamentoByRecursoQueryValidator()
    {
        RuleFor(x => x.IdRecurso).NotEmpty();
    }
}
