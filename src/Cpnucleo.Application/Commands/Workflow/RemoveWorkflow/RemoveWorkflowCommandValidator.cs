namespace Cpnucleo.Application.Commands.Workflow.RemoveWorkflow;

public class RemoveWorkflowCommandValidator : AbstractValidator<RemoveWorkflowCommand>
{
    public RemoveWorkflowCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
