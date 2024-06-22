namespace Application.UseCases.Impediment.GetImpedimentById;

public sealed class GetImpedimentByIdQueryValidator : AbstractValidator<GetImpedimentByIdQuery>
{
    public GetImpedimentByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}
