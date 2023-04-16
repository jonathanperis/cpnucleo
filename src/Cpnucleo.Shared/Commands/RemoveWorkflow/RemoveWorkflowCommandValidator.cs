namespace Cpnucleo.Shared.Commands.RemoveWorkflow;

public sealed class RemoveWorkflowCommandValidator : AbstractValidator<RemoveWorkflowCommand>
{
    public RemoveWorkflowCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
