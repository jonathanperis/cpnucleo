namespace Cpnucleo.Application.Commands.RecursoTarefa;

public sealed class RemoveRecursoTarefaCommandValidator : AbstractValidator<RemoveRecursoTarefaCommand>
{
    public RemoveRecursoTarefaCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
