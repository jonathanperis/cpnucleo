namespace Cpnucleo.Shared.Commands.RemoveProjeto;

public sealed class RemoveProjetoCommandValidator : AbstractValidator<RemoveProjetoCommand>
{
    public RemoveProjetoCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
