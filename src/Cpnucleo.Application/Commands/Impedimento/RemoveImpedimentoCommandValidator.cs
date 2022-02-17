namespace Cpnucleo.Application.Commands.Impedimento;

public class RemoveImpedimentoCommandValidator : AbstractValidator<RemoveImpedimentoCommand>
{
    public RemoveImpedimentoCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
