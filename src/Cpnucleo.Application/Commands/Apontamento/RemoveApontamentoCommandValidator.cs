namespace Cpnucleo.Application.Commands.Apontamento;

public sealed class RemoveApontamentoCommandValidator : AbstractValidator<RemoveApontamentoCommand>
{
    public RemoveApontamentoCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}