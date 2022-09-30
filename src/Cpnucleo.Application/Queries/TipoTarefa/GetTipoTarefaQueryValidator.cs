namespace Cpnucleo.Application.Queries.TipoTarefa;

public sealed class GetTipoTarefaQueryValidator : AbstractValidator<GetTipoTarefaQuery>
{
    public GetTipoTarefaQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
