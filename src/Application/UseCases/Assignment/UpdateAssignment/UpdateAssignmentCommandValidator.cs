namespace Application.UseCases.Assignment.UpdateAssignment;

public sealed class UpdateAssignmentCommandValidator : AbstractValidator<UpdateAssignmentCommand>
{
    public UpdateAssignmentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start Date is required.");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End Date is required.");

        RuleFor(x => x.AmountHours)
            .GreaterThan((int)0).WithMessage("Amount hours is required.");

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
