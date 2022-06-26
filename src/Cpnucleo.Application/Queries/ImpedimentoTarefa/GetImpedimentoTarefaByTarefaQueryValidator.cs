namespace Cpnucleo.Application.Queries.ImpedimentoTarefa;

public class GetImpedimentoTarefaByTarefaQueryValidator : AbstractValidator<GetImpedimentoTarefaByTarefaQuery>
{
    public GetImpedimentoTarefaByTarefaQueryValidator()
    {
        RuleFor(x => x.IdTarefa).NotEmpty();
    }
}
