namespace Cpnucleo.Application.Commands.UpdateRecursoTarefa;

public sealed class UpdateRecursoTarefaCommandValidator : AbstractValidator<UpdateRecursoTarefaCommand>
{
    public UpdateRecursoTarefaCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.IdRecurso).NotEmpty();
        RuleFor(x => x.IdTarefa).NotEmpty();
    }
}
