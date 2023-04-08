using Cpnucleo.Shared.Commands.RemoveTipoTarefa;

namespace Cpnucleo.Application.Commands.RemoveTipoTarefa;

public sealed class RemoveTipoTarefaCommandValidator : AbstractValidator<RemoveTipoTarefaCommand>
{
    public RemoveTipoTarefaCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
