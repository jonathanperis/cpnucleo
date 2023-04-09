namespace Cpnucleo.Application.Commands.CreateRecursoTarefa;

public sealed class CreateRecursoTarefaCommandValidator : AbstractValidator<CreateRecursoTarefaCommand>
{
    public CreateRecursoTarefaCommandValidator()
    {
        RuleFor(x => x.IdRecurso).NotEmpty();
        RuleFor(x => x.IdTarefa).NotEmpty();
    }
}
