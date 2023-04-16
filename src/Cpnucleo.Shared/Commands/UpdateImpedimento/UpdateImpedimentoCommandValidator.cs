namespace Cpnucleo.Shared.Commands.UpdateImpedimento;

public sealed class UpdateImpedimentoCommandValidator : AbstractValidator<UpdateImpedimentoCommand>
{
    public UpdateImpedimentoCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Necessário informar o Id do Impedimento");

        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("Necessário informar o Nome do Impedimento");

        RuleFor(x => x.Nome)
            .MaximumLength(50)
            .WithMessage("Nome pode conter no máximo 50 caractéres");
    }
}
