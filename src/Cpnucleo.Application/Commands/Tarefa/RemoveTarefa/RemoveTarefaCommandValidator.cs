namespace Cpnucleo.Application.Commands.Tarefa.RemoveTarefa;

public class RemoveTarefaCommandValidator : AbstractValidator<RemoveTarefaCommand>
{
    public RemoveTarefaCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
