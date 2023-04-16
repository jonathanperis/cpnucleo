namespace Cpnucleo.Shared.Commands.UpdateImpedimento;

public sealed class UpdateImpedimentoCommandValidator : AbstractValidator<UpdateImpedimentoCommand>
{
    public UpdateImpedimentoCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Nome).NotEmpty();
        RuleFor(x => x.Nome).MaximumLength(50);
    }
}
