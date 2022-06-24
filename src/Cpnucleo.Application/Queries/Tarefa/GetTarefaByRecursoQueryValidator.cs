using Cpnucleo.Shared.Queries.Tarefa;

namespace Cpnucleo.Application.Queries.Tarefa;

public class GetTarefaByRecursoQueryValidator : AbstractValidator<GetTarefaByRecursoQuery>
{
    public GetTarefaByRecursoQueryValidator()
    {
        RuleFor(x => x.IdRecurso).NotEmpty();
    }
}
