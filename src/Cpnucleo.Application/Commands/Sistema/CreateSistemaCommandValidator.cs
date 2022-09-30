namespace Cpnucleo.Application.Commands.Sistema;

public sealed class CreateSistemaCommandValidator : AbstractValidator<CreateSistemaCommand>
{
    public CreateSistemaCommandValidator()
    {
        RuleFor(x => x.Nome).NotEmpty();
        RuleFor(x => x.Nome).MaximumLength(50);
        RuleFor(x => x.Descricao).NotEmpty();
        RuleFor(x => x.Descricao).MaximumLength(450);
    }
}
