namespace Cpnucleo.Application.Commands.Impedimento;

public sealed class RemoveImpedimentoCommandValidator : AbstractValidator<RemoveImpedimentoCommand>
{
    public RemoveImpedimentoCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
