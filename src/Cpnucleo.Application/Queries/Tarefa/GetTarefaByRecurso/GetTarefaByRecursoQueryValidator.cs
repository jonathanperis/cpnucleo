namespace Cpnucleo.Application.Queries.Tarefa.GetTarefaByRecurso;

public class GetTarefaByRecursoQueryValidator : AbstractValidator<GetTarefaByRecursoQuery>
{
    public GetTarefaByRecursoQueryValidator()
    {
        RuleFor(x => x.IdRecurso).NotEmpty();
    }
}
