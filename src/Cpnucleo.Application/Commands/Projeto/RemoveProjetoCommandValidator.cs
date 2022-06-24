using Cpnucleo.Shared.Commands.Projeto;

namespace Cpnucleo.Application.Commands.Projeto;

public class RemoveProjetoCommandValidator : AbstractValidator<RemoveProjetoCommand>
{
    public RemoveProjetoCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
