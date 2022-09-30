namespace Cpnucleo.Application.Commands.Recurso;

public sealed class RemoveRecursoCommandValidator : AbstractValidator<RemoveRecursoCommand>
{
    public RemoveRecursoCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
