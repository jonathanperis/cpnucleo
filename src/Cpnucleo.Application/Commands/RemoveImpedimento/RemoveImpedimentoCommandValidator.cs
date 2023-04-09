namespace Cpnucleo.Application.Commands.RemoveImpedimento;

public sealed class RemoveImpedimentoCommandValidator : AbstractValidator<RemoveImpedimentoCommand>
{
    public RemoveImpedimentoCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
