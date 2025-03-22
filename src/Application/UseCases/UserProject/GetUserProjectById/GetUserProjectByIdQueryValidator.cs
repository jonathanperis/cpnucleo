namespace Application.UseCases.UserProject.GetUserProjectById;

public sealed class GetUserProjectByIdQueryValidator : AbstractValidator<GetUserProjectByIdQuery>
{
    public GetUserProjectByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}
