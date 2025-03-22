namespace Application.UseCases.Workflow.CreateWorkflow;

public sealed class CreateWorkflowCommandValidator : AbstractValidator<CreateWorkflowCommand>
{
    public CreateWorkflowCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");

        RuleFor(x => x.Order)
            .GreaterThan((int)0).WithMessage("Order must be greater than 0.");
    }
}
