namespace Cpnucleo.Application.Commands.Recurso;

public class RemoveRecursoCommandValidator : AbstractValidator<RemoveRecursoCommand>
{
    public RemoveRecursoCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
