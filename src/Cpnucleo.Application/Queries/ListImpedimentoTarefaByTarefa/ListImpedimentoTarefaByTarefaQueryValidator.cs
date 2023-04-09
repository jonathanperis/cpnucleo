namespace Cpnucleo.Application.Queries.ListImpedimentoTarefaByTarefa;

public sealed class ListImpedimentoTarefaByTarefaQueryValidator : AbstractValidator<ListImpedimentoTarefaByTarefaQuery>
{
    public ListImpedimentoTarefaByTarefaQueryValidator()
    {
        RuleFor(x => x.IdTarefa).NotEmpty();
    }
}
