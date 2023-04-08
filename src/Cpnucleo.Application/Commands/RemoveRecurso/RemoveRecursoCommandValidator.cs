using Cpnucleo.Shared.Commands.RemoveRecurso;

namespace Cpnucleo.Application.Commands.RemoveRecurso;

public sealed class RemoveRecursoCommandValidator : AbstractValidator<RemoveRecursoCommand>
{
    public RemoveRecursoCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
