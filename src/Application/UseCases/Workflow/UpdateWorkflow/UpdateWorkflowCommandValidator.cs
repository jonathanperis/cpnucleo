namespace Application.UseCases.Workflow.UpdateWorkflow;

public sealed class UpdateWorkflowCommandValidator : AbstractValidator<UpdateWorkflowCommand>
{
    public UpdateWorkflowCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");

        RuleFor(x => x.Order)
            .GreaterThan((int)0).WithMessage("Order must be greater than 0.");
    }
}
