namespace Cpnucleo.Application.Commands.Projeto;

public sealed class RemoveProjetoCommandValidator : AbstractValidator<RemoveProjetoCommand>
{
    public RemoveProjetoCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
