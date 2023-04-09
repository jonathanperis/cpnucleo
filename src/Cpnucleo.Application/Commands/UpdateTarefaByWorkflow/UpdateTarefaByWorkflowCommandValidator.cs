namespace Cpnucleo.Application.Commands.UpdateTarefaByWorkflow;

public sealed class UpdateTarefaByWorkflowCommandValidator : AbstractValidator<UpdateTarefaByWorkflowCommand>
{
    public UpdateTarefaByWorkflowCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.IdWorkflow).NotEmpty();
    }
}
