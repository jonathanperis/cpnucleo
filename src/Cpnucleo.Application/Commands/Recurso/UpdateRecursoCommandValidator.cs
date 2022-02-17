namespace Cpnucleo.Application.Commands.Recurso;

public class UpdateRecursoCommandValidator : AbstractValidator<UpdateRecursoCommand>
{
    public UpdateRecursoCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Nome).NotEmpty();
        RuleFor(x => x.Nome).MaximumLength(50);
        RuleFor(x => x.Senha).NotEmpty();
        RuleFor(x => x.Senha).MaximumLength(50);
    }
}
