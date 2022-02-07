namespace Cpnucleo.Application.Queries.ImpedimentoTarefa.GetImpedimentoTarefa;

public class GetImpedimentoTarefaQueryValidator : AbstractValidator<GetImpedimentoTarefaQuery>
{
    public GetImpedimentoTarefaQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
