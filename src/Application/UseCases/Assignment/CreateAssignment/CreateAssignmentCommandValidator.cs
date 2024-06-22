namespace Application.UseCases.Assignment.CreateAssignment;

public sealed class CreateAssignmentCommandValidator : AbstractValidator<CreateAssignmentCommand>
{
    public CreateAssignmentCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.");

        RuleFor(x => x.StartDate)
            .LessThan(x => x.EndDate).WithMessage("StartDate must be less than EndDate.");

        RuleFor(x => x.AmountHours)
            .GreaterThan((byte)0).WithMessage("AmountHours must be greater than 0.");

        RuleFor(x => x.ProjectId)
            .NotEmpty().WithMessage("ProjectId is required.");

        RuleFor(x => x.WorkflowId)
            .NotEmpty().WithMessage("WorkflowId is required.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.AssignmentTypeId)
            .NotEmpty().WithMessage("AssignmentTypeId is required.");
    }
}
