namespace Application.UseCases.UserAssignment.UpdateUserAssignment;

public sealed class UpdateUserAssignmentCommandValidator : AbstractValidator<UpdateUserAssignmentCommand>
{
    public UpdateUserAssignmentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.AssignmentId)
            .NotEmpty().WithMessage("AssignmentId is required.");
    }
}
