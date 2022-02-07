namespace Cpnucleo.Application.Commands.Impedimento.RemoveImpedimento;

public class RemoveImpedimentoCommandValidator : AbstractValidator<RemoveImpedimentoCommand>
{
    public RemoveImpedimentoCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
