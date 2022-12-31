namespace Cpnucleo.Application.Queries.Tarefa;

public sealed class GetTarefaByRecursoQueryValidator : AbstractValidator<ListTarefaByRecursoQuery>
{
    public GetTarefaByRecursoQueryValidator()
    {
        RuleFor(x => x.IdRecurso).NotEmpty();
    }
}
