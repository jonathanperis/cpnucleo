namespace Cpnucleo.Application.Commands.Projeto;

public sealed class CreateProjetoCommandValidator : AbstractValidator<CreateProjetoCommand>
{
    public CreateProjetoCommandValidator()
    {
        RuleFor(x => x.Nome).NotEmpty();
        RuleFor(x => x.Nome).MaximumLength(50);
        RuleFor(x => x.IdSistema).NotEmpty();
    }
}
