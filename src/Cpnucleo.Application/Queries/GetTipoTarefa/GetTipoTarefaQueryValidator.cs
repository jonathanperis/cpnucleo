using Cpnucleo.Shared.Queries.GetTipoTarefa;

namespace Cpnucleo.Application.Queries.GetTipoTarefa;

public sealed class GetTipoTarefaQueryValidator : AbstractValidator<GetTipoTarefaQuery>
{
    public GetTipoTarefaQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
