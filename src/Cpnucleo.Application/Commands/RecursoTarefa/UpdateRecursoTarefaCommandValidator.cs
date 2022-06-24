using Cpnucleo.Shared.Commands.RecursoTarefa;

namespace Cpnucleo.Application.Commands.RecursoTarefa;

public class UpdateRecursoTarefaCommandValidator : AbstractValidator<UpdateRecursoTarefaCommand>
{
    public UpdateRecursoTarefaCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.IdRecurso).NotEmpty();
        RuleFor(x => x.IdTarefa).NotEmpty();
    }
}
