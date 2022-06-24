using Cpnucleo.Shared.Queries.RecursoTarefa;

namespace Cpnucleo.Application.Queries.RecursoTarefa;

public class GetRecursoTarefaQueryValidator : AbstractValidator<GetRecursoTarefaQuery>
{
    public GetRecursoTarefaQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
