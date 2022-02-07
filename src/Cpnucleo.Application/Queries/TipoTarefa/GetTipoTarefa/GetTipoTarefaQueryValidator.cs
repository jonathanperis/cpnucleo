namespace Cpnucleo.Application.Queries.TipoTarefa.GetTipoTarefa;

public class GetTipoTarefaQueryValidator : AbstractValidator<GetTipoTarefaQuery>
{
    public GetTipoTarefaQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
