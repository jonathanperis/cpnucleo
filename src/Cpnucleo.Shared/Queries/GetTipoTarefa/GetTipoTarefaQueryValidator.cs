namespace Cpnucleo.Shared.Queries.GetTipoTarefa;

public sealed class GetTipoTarefaQueryValidator : AbstractValidator<GetTipoTarefaQuery>
{
    public GetTipoTarefaQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Necessário informar o Id do Tipo Tarefa");
    }
}
