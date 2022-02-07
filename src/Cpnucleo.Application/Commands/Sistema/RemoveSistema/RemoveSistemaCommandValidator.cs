namespace Cpnucleo.Application.Commands.Sistema.RemoveSistema;

public class RemoveSistemaCommandValidator : AbstractValidator<RemoveSistemaCommand>
{
    public RemoveSistemaCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
