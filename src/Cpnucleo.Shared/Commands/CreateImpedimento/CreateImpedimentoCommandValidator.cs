namespace Cpnucleo.Shared.Commands.CreateImpedimento;

public sealed class CreateImpedimentoCommandValidator : AbstractValidator<CreateImpedimentoCommand>
{
    public CreateImpedimentoCommandValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("Necessário informar o Nome do Impedimento");

        RuleFor(x => x.Nome)
            .MaximumLength(50)
            .WithMessage("Nome pode conter no máximo 50 caractéres");
    }
}
