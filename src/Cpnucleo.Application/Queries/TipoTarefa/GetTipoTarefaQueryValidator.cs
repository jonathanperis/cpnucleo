using Cpnucleo.Shared.Queries.TipoTarefa;

namespace Cpnucleo.Application.Queries.TipoTarefa;

public class GetTipoTarefaQueryValidator : AbstractValidator<GetTipoTarefaQuery>
{
    public GetTipoTarefaQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
