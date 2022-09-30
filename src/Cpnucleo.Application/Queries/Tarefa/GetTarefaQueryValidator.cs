namespace Cpnucleo.Application.Queries.Tarefa;

public sealed class GetTarefaQueryValidator : AbstractValidator<GetTarefaQuery>
{
    public GetTarefaQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
