namespace Cpnucleo.Application.Queries.GetImpedimentoTarefa;

public sealed class GetImpedimentoTarefaQueryValidator : AbstractValidator<GetImpedimentoTarefaQuery>
{
    public GetImpedimentoTarefaQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
