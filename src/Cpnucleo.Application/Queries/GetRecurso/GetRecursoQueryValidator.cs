using Cpnucleo.Shared.Queries.GetRecurso;

namespace Cpnucleo.Application.Queries.GetRecurso;

public sealed class GetRecursoQueryValidator : AbstractValidator<GetRecursoQuery>
{
    public GetRecursoQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
