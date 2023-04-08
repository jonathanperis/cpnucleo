using Cpnucleo.Shared.Commands.RemoveSistema;

namespace Cpnucleo.Application.Commands.RemoveSistema;

public sealed class RemoveSistemaCommandValidator : AbstractValidator<RemoveSistemaCommand>
{
    public RemoveSistemaCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
