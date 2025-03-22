namespace Application.UseCases.AssignmentType.CreateAssignmentType;

public sealed class CreateAssignmentTypeCommandValidator : AbstractValidator<CreateAssignmentTypeCommand>
{
    public CreateAssignmentTypeCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");
    }
}
