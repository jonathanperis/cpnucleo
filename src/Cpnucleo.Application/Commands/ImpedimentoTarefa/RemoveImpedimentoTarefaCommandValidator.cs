namespace Cpnucleo.Application.Commands.ImpedimentoTarefa;

public sealed class RemoveImpedimentoTarefaCommandValidator : AbstractValidator<RemoveImpedimentoTarefaCommand>
{
    public RemoveImpedimentoTarefaCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
