namespace Cpnucleo.Application.Commands.Impedimento;

public sealed class CreateImpedimentoCommandValidator : AbstractValidator<CreateImpedimentoCommand>
{
    public CreateImpedimentoCommandValidator()
    {
        RuleFor(x => x.Nome).NotEmpty();
        RuleFor(x => x.Nome).MaximumLength(50);
    }
}
