using Cpnucleo.Shared.Commands.UpdateRecurso;

namespace Cpnucleo.Application.Commands.UpdateRecurso;

public sealed class UpdateRecursoCommandValidator : AbstractValidator<UpdateRecursoCommand>
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
