namespace Cpnucleo.Application.Queries.ImpedimentoTarefa;

public sealed class GetImpedimentoTarefaQueryValidator : AbstractValidator<GetImpedimentoTarefaQuery>
{
    public GetImpedimentoTarefaQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
