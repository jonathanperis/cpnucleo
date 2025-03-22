namespace Application.UseCases.UserAssignment.CreateUserAssignment;

public sealed class CreateUserAssignmentCommandValidator : AbstractValidator<CreateUserAssignmentCommand>
{
    public CreateUserAssignmentCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.AssignmentId)
            .NotEmpty().WithMessage("AssignmentId is required.");
    }
}
