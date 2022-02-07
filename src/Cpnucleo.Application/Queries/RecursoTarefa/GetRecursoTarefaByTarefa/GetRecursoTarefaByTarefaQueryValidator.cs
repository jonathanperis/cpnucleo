namespace Cpnucleo.Application.Queries.RecursoTarefa.GetRecursoTarefaByTarefa;

public class GetRecursoTarefaByTarefaQueryValidator : AbstractValidator<GetRecursoTarefaByTarefaQuery>
{
    public GetRecursoTarefaByTarefaQueryValidator()
    {
        RuleFor(x => x.IdTarefa).NotEmpty();
    }
}
