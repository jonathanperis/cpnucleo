namespace Cpnucleo.Shared.Commands.CreateRecurso;

public sealed class CreateRecursoCommandValidator : AbstractValidator<CreateRecursoCommand>
{
    public CreateRecursoCommandValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("Necessário informar o Nome do Recurso");

        RuleFor(x => x.Nome)
            .MaximumLength(50)
            .WithMessage("Nome pode conter no máximo 50 caractéres");

        RuleFor(x => x.Login)
            .NotEmpty()
            .WithMessage("Necessário informar o Login do Recurso");

        RuleFor(x => x.Login)
            .MaximumLength(50)
            .WithMessage("Login pode conter no máximo 50 caractéres");

        RuleFor(x => x.Senha)
            .NotEmpty()
            .WithMessage("Necessário informar a Senha do Recurso");

        RuleFor(x => x.Senha)
            .MaximumLength(50)
            .WithMessage("Senha pode conter no máximo 50 caractéres");
    }
}
