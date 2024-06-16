namespace Application.UseCases.System.GetSystemById;

public sealed class GetSystemByIdQueryValidator : AbstractValidator<GetSystemByIdQuery>
{
    public GetSystemByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}
