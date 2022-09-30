namespace Cpnucleo.Application.Queries.Tarefa;

public sealed class GetTarefaByRecursoQueryValidator : AbstractValidator<GetTarefaByRecursoQuery>
{
    public GetTarefaByRecursoQueryValidator()
    {
        RuleFor(x => x.IdRecurso).NotEmpty();
    }
}
