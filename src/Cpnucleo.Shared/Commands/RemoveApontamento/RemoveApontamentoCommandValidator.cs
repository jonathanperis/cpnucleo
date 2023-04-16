namespace Cpnucleo.Shared.Commands.RemoveApontamento;

public sealed class RemoveApontamentoCommandValidator : AbstractValidator<RemoveApontamentoCommand>
{
    public RemoveApontamentoCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}