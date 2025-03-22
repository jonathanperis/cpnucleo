namespace Application.UseCases.AssignmentType.GetAssignmentTypeById;

public sealed class GetAssignmentTypeByIdQueryValidator : AbstractValidator<GetAssignmentTypeByIdQuery>
{
    public GetAssignmentTypeByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}
