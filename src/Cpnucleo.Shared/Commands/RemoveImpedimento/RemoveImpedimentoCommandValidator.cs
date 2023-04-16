namespace Cpnucleo.Shared.Commands.RemoveImpedimento;

public sealed class RemoveImpedimentoCommandValidator : AbstractValidator<RemoveImpedimentoCommand>
{
    public RemoveImpedimentoCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Necessário informar o Id do Impedimento");
    }
}
