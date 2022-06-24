using Cpnucleo.Shared.Queries.ImpedimentoTarefa;

namespace Cpnucleo.Application.Queries.ImpedimentoTarefa;

public class GetImpedimentoTarefaQueryValidator : AbstractValidator<GetImpedimentoTarefaQuery>
{
    public GetImpedimentoTarefaQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
