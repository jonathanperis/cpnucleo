namespace Cpnucleo.Shared.Commands.CreateRecursoTarefa;

public sealed class CreateRecursoTarefaCommandValidator : AbstractValidator<CreateRecursoTarefaCommand>
{
    public CreateRecursoTarefaCommandValidator()
    {
        RuleFor(x => x.IdRecurso)
            .NotEmpty()
            .WithMessage("Recurso Tarefa deve conter um Recurso");

        RuleFor(x => x.IdTarefa)
            .NotEmpty()
            .WithMessage("Recurso Tarefa deve conter uma Tarefa");
    }
}
