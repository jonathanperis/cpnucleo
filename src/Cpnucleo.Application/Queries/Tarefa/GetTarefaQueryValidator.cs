using Cpnucleo.Shared.Queries.Tarefa;

namespace Cpnucleo.Application.Queries.Tarefa;

public class GetTarefaQueryValidator : AbstractValidator<GetTarefaQuery>
{
    public GetTarefaQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
