namespace Application.UseCases.UserAssignment.RemoveUserAssignment;

public sealed class RemoveUserAssignmentCommandValidator : AbstractValidator<RemoveUserAssignmentCommand>
{
    public RemoveUserAssignmentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}
