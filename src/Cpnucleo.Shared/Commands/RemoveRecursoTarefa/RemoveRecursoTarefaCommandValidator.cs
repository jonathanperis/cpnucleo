namespace Cpnucleo.Shared.Commands.RemoveRecursoTarefa;

public sealed class RemoveRecursoTarefaCommandValidator : AbstractValidator<RemoveRecursoTarefaCommand>
{
    public RemoveRecursoTarefaCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Necessário informar o Id do Recurso Tarefa");
    }
}
