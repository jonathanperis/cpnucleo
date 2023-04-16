namespace Cpnucleo.Shared.Commands.RemoveTarefa;

public sealed class RemoveTarefaCommandValidator : AbstractValidator<RemoveTarefaCommand>
{
    public RemoveTarefaCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Necessário informar o Id da Tarefa");
    }
}
