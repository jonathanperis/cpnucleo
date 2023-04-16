namespace Cpnucleo.Shared.Queries.GetTarefa;

public sealed class GetTarefaQueryValidator : AbstractValidator<GetTarefaQuery>
{
    public GetTarefaQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Necessário informar o Id da Tarefa");
    }
}
