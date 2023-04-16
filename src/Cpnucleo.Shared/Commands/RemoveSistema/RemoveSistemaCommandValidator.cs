namespace Cpnucleo.Shared.Commands.RemoveSistema;

public sealed class RemoveSistemaCommandValidator : AbstractValidator<RemoveSistemaCommand>
{
    public RemoveSistemaCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Necessário informar o Id do Sistema");
    }
}
