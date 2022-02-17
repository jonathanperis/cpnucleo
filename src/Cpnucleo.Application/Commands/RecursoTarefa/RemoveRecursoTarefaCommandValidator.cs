namespace Cpnucleo.Application.Commands.RecursoTarefa;

public class RemoveRecursoTarefaCommandValidator : AbstractValidator<RemoveRecursoTarefaCommand>
{
    public RemoveRecursoTarefaCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
