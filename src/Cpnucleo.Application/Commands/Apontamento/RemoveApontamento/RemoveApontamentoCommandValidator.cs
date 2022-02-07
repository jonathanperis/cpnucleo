namespace Cpnucleo.Application.Commands.Apontamento.RemoveApontamento;

public class RemoveApontamentoCommandValidator : AbstractValidator<RemoveApontamentoCommand>
{
    public RemoveApontamentoCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}