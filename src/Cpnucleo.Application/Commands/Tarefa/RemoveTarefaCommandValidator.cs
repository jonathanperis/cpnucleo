namespace Cpnucleo.Application.Commands.Tarefa;

public sealed class RemoveTarefaCommandValidator : AbstractValidator<RemoveTarefaCommand>
{
    public RemoveTarefaCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
