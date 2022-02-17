namespace Cpnucleo.Application.Commands.Tarefa;

public class RemoveTarefaCommandValidator : AbstractValidator<RemoveTarefaCommand>
{
    public RemoveTarefaCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
