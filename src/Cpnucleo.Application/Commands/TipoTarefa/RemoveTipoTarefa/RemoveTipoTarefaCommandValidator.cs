namespace Cpnucleo.Application.Commands.TipoTarefa.RemoveTipoTarefa;

public class RemoveTipoTarefaCommandValidator : AbstractValidator<RemoveTipoTarefaCommand>
{
    public RemoveTipoTarefaCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
