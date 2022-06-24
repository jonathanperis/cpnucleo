using Cpnucleo.Shared.Commands.Apontamento;

namespace Cpnucleo.Application.Commands.Apontamento;

public class RemoveApontamentoCommandValidator : AbstractValidator<RemoveApontamentoCommand>
{
    public RemoveApontamentoCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}