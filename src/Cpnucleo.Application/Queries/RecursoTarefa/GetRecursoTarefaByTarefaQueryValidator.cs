namespace Cpnucleo.Application.Queries.RecursoTarefa;

public sealed class GetRecursoTarefaByTarefaQueryValidator : AbstractValidator<GetRecursoTarefaByTarefaQuery>
{
    public GetRecursoTarefaByTarefaQueryValidator()
    {
        RuleFor(x => x.IdTarefa).NotEmpty();
    }
}
