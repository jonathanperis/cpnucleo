namespace Cpnucleo.Shared.Queries.ListRecursoTarefaByTarefa;

public sealed class ListRecursoTarefaByTarefaQueryValidator : AbstractValidator<ListRecursoTarefaByTarefaQuery>
{
    public ListRecursoTarefaByTarefaQueryValidator()
    {
        RuleFor(x => x.IdTarefa).NotEmpty();
    }
}
