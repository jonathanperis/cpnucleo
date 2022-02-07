namespace Cpnucleo.Application.Commands.RecursoTarefa.CreateRecursoTarefa;

public class CreateRecursoTarefaCommandValidator : AbstractValidator<CreateRecursoTarefaCommand>
{
    public CreateRecursoTarefaCommandValidator()
    {
        RuleFor(x => x.IdRecurso).NotEmpty();
        RuleFor(x => x.IdTarefa).NotEmpty();
    }
}
