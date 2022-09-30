namespace Cpnucleo.Application.Queries.ImpedimentoTarefa;

public sealed class GetImpedimentoTarefaByTarefaQueryValidator : AbstractValidator<GetImpedimentoTarefaByTarefaQuery>
{
    public GetImpedimentoTarefaByTarefaQueryValidator()
    {
        RuleFor(x => x.IdTarefa).NotEmpty();
    }
}
