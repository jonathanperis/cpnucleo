using Cpnucleo.Shared.Queries.GetRecursoTarefa;

namespace Cpnucleo.Application.Queries.GetRecursoTarefa;

public sealed class GetRecursoTarefaQueryValidator : AbstractValidator<GetRecursoTarefaQuery>
{
    public GetRecursoTarefaQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
