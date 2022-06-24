using Cpnucleo.Shared.Commands.Sistema;

namespace Cpnucleo.Application.Commands.Sistema;

public class RemoveSistemaCommandValidator : AbstractValidator<RemoveSistemaCommand>
{
    public RemoveSistemaCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
