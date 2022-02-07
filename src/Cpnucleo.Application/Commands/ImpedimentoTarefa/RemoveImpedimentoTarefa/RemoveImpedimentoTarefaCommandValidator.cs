namespace Cpnucleo.Application.Commands.ImpedimentoTarefa.RemoveImpedimentoTarefa;

public class RemoveImpedimentoTarefaCommandValidator : AbstractValidator<RemoveImpedimentoTarefaCommand>
{
    public RemoveImpedimentoTarefaCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
