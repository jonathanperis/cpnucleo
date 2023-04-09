namespace Cpnucleo.Application.Commands.RemoveRecursoProjeto;

public sealed class RemoveRecursoProjetoCommandValidator : AbstractValidator<RemoveRecursoProjetoCommand>
{
    public RemoveRecursoProjetoCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
