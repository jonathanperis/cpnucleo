using Cpnucleo.Shared.Queries.RecursoTarefa;

namespace Cpnucleo.Application.Queries.RecursoTarefa;

public class GetRecursoTarefaByTarefaQueryValidator : AbstractValidator<GetRecursoTarefaByTarefaQuery>
{
    public GetRecursoTarefaByTarefaQueryValidator()
    {
        RuleFor(x => x.IdTarefa).NotEmpty();
    }
}
