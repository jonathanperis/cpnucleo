namespace Application.UseCases.AssignmentType.UpdateAssignmentType;

public sealed class UpdateAssignmentTypeCommandValidator : AbstractValidator<UpdateAssignmentTypeCommand>
{
    public UpdateAssignmentTypeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");
    }
}
