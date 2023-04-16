namespace Cpnucleo.Shared.Queries.ListTarefaByRecurso;

public sealed class ListTarefaByRecursoQueryValidator : AbstractValidator<ListTarefaByRecursoQuery>
{
    public ListTarefaByRecursoQueryValidator()
    {
        RuleFor(x => x.IdRecurso)
            .NotEmpty()
            .WithMessage("Tarefa deve conter um Recurso");
    }
}
