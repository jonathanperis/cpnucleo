namespace Application.UseCases.AssignmentImpediment.UpdateAssignmentImpediment;

public sealed class UpdateAssignmentImpedimentCommandValidator : AbstractValidator<UpdateAssignmentImpedimentCommand>
{
    public UpdateAssignmentImpedimentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.");

        RuleFor(x => x.AssignmentId)
            .NotEmpty().WithMessage("AssignmentId is required.");

        RuleFor(x => x.ImpedimentId)
            .NotEmpty().WithMessage("ImpedimentId is required.");
    }
}
