namespace Cpnucleo.Application.Commands.RecursoTarefa.RemoveRecursoTarefa;

public class RemoveRecursoTarefaCommandValidator : AbstractValidator<RemoveRecursoTarefaCommand>
{
    public RemoveRecursoTarefaCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
