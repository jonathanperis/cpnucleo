namespace Cpnucleo.Application.Commands.Impedimento.CreateImpedimento;

public class CreateImpedimentoCommandValidator : AbstractValidator<CreateImpedimentoCommand>
{
    public CreateImpedimentoCommandValidator()
    {
        RuleFor(x => x.Nome).NotEmpty();
        RuleFor(x => x.Nome).MaximumLength(50);
    }
}
