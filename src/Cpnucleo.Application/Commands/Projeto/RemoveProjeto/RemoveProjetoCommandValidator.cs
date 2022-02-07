namespace Cpnucleo.Application.Commands.Projeto.RemoveProjeto;

public class RemoveProjetoCommandValidator : AbstractValidator<RemoveProjetoCommand>
{
    public RemoveProjetoCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
