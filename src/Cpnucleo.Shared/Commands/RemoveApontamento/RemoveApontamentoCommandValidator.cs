namespace Cpnucleo.Shared.Commands.RemoveApontamento;

public sealed class RemoveApontamentoCommandValidator : AbstractValidator<RemoveApontamentoCommand>
{
    public RemoveApontamentoCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Necessário informar o Id do Apontamento");
    }
}