namespace Application.UseCases.AssignmentImpediment.CreateAssignmentImpediment;

public sealed class CreateAssignmentImpedimentCommandValidator : AbstractValidator<CreateAssignmentImpedimentCommand>
{
    public CreateAssignmentImpedimentCommandValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.");

        RuleFor(x => x.AssignmentId)
            .NotEmpty().WithMessage("AssignmentId is required.");

        RuleFor(x => x.ImpedimentId)
            .NotEmpty().WithMessage("ImpedimentId is required.");
    }
}
