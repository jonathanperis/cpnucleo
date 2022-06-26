namespace Cpnucleo.Application.Commands.RecursoProjeto;

public class RemoveRecursoProjetoCommandValidator : AbstractValidator<RemoveRecursoProjetoCommand>
{
    public RemoveRecursoProjetoCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
