namespace Application.UseCases.AssignmentImpediment.RemoveAssignmentImpediment;

public sealed class RemoveAssignmentImpedimentCommandValidator : AbstractValidator<RemoveAssignmentImpedimentCommand>
{
    public RemoveAssignmentImpedimentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}
