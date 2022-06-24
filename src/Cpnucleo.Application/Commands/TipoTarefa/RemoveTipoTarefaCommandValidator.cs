using Cpnucleo.Shared.Commands.TipoTarefa;

namespace Cpnucleo.Application.Commands.TipoTarefa;

public class RemoveTipoTarefaCommandValidator : AbstractValidator<RemoveTipoTarefaCommand>
{
    public RemoveTipoTarefaCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
