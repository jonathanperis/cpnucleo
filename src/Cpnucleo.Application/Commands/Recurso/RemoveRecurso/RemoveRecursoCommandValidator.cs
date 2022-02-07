namespace Cpnucleo.Application.Commands.Recurso.RemoveRecurso;

public class RemoveRecursoCommandValidator : AbstractValidator<RemoveRecursoCommand>
{
    public RemoveRecursoCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
