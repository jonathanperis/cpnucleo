using Cpnucleo.Shared.Commands.RemoveTarefa;

namespace Cpnucleo.Application.Commands.RemoveTarefa;

public sealed class RemoveTarefaCommandValidator : AbstractValidator<RemoveTarefaCommand>
{
    public RemoveTarefaCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
