using Cpnucleo.Shared.Queries.ListTarefaByRecurso;

namespace Cpnucleo.Application.Queries.ListTarefaByRecurso;

public sealed class ListTarefaByRecursoQueryValidator : AbstractValidator<ListTarefaByRecursoQuery>
{
    public ListTarefaByRecursoQueryValidator()
    {
        RuleFor(x => x.IdRecurso).NotEmpty();
    }
}
