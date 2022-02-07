namespace Cpnucleo.Application.Commands.RecursoProjeto.RemoveRecursoProjeto;

public class RemoveRecursoProjetoCommandValidator : AbstractValidator<RemoveRecursoProjetoCommand>
{
    public RemoveRecursoProjetoCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
