namespace Cpnucleo.Application.Queries.ImpedimentoTarefa;

public sealed class GetImpedimentoTarefaByTarefaQueryValidator : AbstractValidator<ListImpedimentoTarefaByTarefaQuery>
{
    public GetImpedimentoTarefaByTarefaQueryValidator()
    {
        RuleFor(x => x.IdTarefa).NotEmpty();
    }
}
