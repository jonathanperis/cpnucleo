namespace Cpnucleo.Shared.Commands.UpdateRecursoTarefa;

public sealed class UpdateRecursoTarefaCommandValidator : AbstractValidator<UpdateRecursoTarefaCommand>
{
    public UpdateRecursoTarefaCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Necessário informar o Id do Recurso Tarefa");

        RuleFor(x => x.IdRecurso)
            .NotEmpty()
            .WithMessage("Recurso Tarefa deve conter um Recurso");

        RuleFor(x => x.IdTarefa)
            .NotEmpty()
            .WithMessage("Recurso Tarefa deve conter uma Tarefa");
    }
}
