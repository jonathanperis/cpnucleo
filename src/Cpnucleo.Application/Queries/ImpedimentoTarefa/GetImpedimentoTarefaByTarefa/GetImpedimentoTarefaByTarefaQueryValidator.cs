namespace Cpnucleo.Application.Queries.ImpedimentoTarefa.GetImpedimentoTarefaByTarefa;

public class GetImpedimentoTarefaByTarefaQueryValidator : AbstractValidator<GetImpedimentoTarefaByTarefaQuery>
{
    public GetImpedimentoTarefaByTarefaQueryValidator()
    {
        RuleFor(x => x.IdTarefa).NotEmpty();
    }
}
