using Cpnucleo.Shared.Commands.RemoveProjeto;

namespace Cpnucleo.Application.Commands.RemoveProjeto;

public sealed class RemoveProjetoCommandValidator : AbstractValidator<RemoveProjetoCommand>
{
    public RemoveProjetoCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
