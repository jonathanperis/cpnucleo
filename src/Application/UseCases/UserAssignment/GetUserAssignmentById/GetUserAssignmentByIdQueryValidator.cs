namespace Application.UseCases.UserAssignment.GetUserAssignmentById;

public sealed class GetUserAssignmentByIdQueryValidator : AbstractValidator<GetUserAssignmentByIdQuery>
{
    public GetUserAssignmentByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}
