using Cpnucleo.Shared.Commands.Tarefa;

namespace Cpnucleo.Application.Commands.Tarefa;

public class UpdateTarefaByWorkflowCommandValidator : AbstractValidator<UpdateTarefaByWorkflowCommand>
{
    public UpdateTarefaByWorkflowCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.IdWorkflow).NotEmpty();
    }
}
