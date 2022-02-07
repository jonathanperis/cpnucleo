namespace Cpnucleo.Application.Commands.Recurso.CreateRecurso;

public class CreateRecursoCommandValidator : AbstractValidator<CreateRecursoCommand>
{
    public CreateRecursoCommandValidator()
    {
        RuleFor(x => x.Nome).NotEmpty();
        RuleFor(x => x.Nome).MaximumLength(50);
        RuleFor(x => x.Login).NotEmpty();
        RuleFor(x => x.Login).MaximumLength(50);
        RuleFor(x => x.Senha).NotEmpty();
        RuleFor(x => x.Senha).MaximumLength(50);
        RuleFor(x => x.Salt).Empty();
    }
}
