namespace Application.UseCases.Workflow.RemoveWorkflow;

public sealed class RemoveWorkflowCommandValidator : AbstractValidator<RemoveWorkflowCommand>
{
    public RemoveWorkflowCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}
