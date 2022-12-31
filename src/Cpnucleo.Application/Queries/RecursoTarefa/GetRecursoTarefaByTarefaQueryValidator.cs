namespace Cpnucleo.Application.Queries.RecursoTarefa;

public sealed class GetRecursoTarefaByTarefaQueryValidator : AbstractValidator<ListRecursoTarefaByTarefaQuery>
{
    public GetRecursoTarefaByTarefaQueryValidator()
    {
        RuleFor(x => x.IdTarefa).NotEmpty();
    }
}
