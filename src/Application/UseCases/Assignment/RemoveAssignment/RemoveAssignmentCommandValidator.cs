namespace Application.UseCases.Assignment.RemoveAssignment;

public sealed class RemoveAssignmentCommandValidator : AbstractValidator<RemoveAssignmentCommand>
{
    public RemoveAssignmentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}
