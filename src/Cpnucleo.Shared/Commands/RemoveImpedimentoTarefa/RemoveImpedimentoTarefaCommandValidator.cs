namespace Cpnucleo.Shared.Commands.RemoveImpedimentoTarefa;

public sealed class RemoveImpedimentoTarefaCommandValidator : AbstractValidator<RemoveImpedimentoTarefaCommand>
{
    public RemoveImpedimentoTarefaCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Necessário informar o Id do Impedimento Tarefa");
    }
}
