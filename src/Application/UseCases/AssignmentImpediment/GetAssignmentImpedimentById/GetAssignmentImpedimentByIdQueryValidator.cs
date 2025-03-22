namespace Application.UseCases.AssignmentImpediment.GetAssignmentImpedimentById;

public sealed class GetAssignmentImpedimentByIdQueryValidator : AbstractValidator<GetAssignmentImpedimentByIdQuery>
{
    public GetAssignmentImpedimentByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}
