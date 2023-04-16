namespace Cpnucleo.Shared.Commands.CreateSistema;

public sealed class CreateSistemaCommandValidator : AbstractValidator<CreateSistemaCommand>
{
    public CreateSistemaCommandValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("Necessário informar a Nome do Sistema");

        RuleFor(x => x.Nome)
            .MaximumLength(50)
            .WithMessage("Nome pode conter no máximo 50 caractéres");

        RuleFor(x => x.Descricao)
            .NotEmpty()
            .WithMessage("Necessário informar a Descrição do Sistema");

        RuleFor(x => x.Descricao)
            .MaximumLength(450)
            .WithMessage("Descrição pode conter no máximo 450 caractéres");
    }
}
