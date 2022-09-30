namespace Cpnucleo.Application.Commands.Sistema;

public sealed class RemoveSistemaCommandValidator : AbstractValidator<RemoveSistemaCommand>
{
    public RemoveSistemaCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
