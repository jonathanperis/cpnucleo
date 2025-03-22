namespace Application.UseCases.AssignmentType.RemoveAssignmentType;

public sealed class RemoveAssignmentTypeCommandValidator : AbstractValidator<RemoveAssignmentTypeCommand>
{
    public RemoveAssignmentTypeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}
