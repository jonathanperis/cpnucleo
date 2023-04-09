namespace Cpnucleo.Application.Commands.RemoveImpedimentoTarefa;

public sealed class RemoveImpedimentoTarefaCommandValidator : AbstractValidator<RemoveImpedimentoTarefaCommand>
{
    public RemoveImpedimentoTarefaCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
