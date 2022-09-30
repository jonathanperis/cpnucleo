namespace Cpnucleo.Application.Commands.RecursoProjeto;

public sealed class RemoveRecursoProjetoCommandValidator : AbstractValidator<RemoveRecursoProjetoCommand>
{
    public RemoveRecursoProjetoCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
