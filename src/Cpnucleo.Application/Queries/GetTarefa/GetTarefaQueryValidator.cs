namespace Cpnucleo.Application.Queries.GetTarefa;

public sealed class GetTarefaQueryValidator : AbstractValidator<GetTarefaQuery>
{
    public GetTarefaQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
