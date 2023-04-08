using Cpnucleo.Shared.Queries.GetTarefa;

namespace Cpnucleo.Application.Queries.GetTarefa;

public sealed class GetTarefaQueryValidator : AbstractValidator<GetTarefaQuery>
{
    public GetTarefaQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
