namespace Cpnucleo.Shared.Commands.RemoveTipoTarefa;

public sealed class RemoveTipoTarefaCommandValidator : AbstractValidator<RemoveTipoTarefaCommand>
{
    public RemoveTipoTarefaCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
