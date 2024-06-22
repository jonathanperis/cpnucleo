namespace Application.UseCases.Assignment.GetAssignmentById;

public sealed class GetAssignmentByIdQueryValidator : AbstractValidator<GetAssignmentByIdQuery>
{
    public GetAssignmentByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
    }
}
