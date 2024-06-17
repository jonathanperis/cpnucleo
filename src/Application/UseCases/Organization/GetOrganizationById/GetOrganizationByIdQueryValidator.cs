namespace Application.UseCases.Organization.GetOrganizationById;

public sealed class GetOrganizationByIdQueryValidator : AbstractValidator<GetOrganizationByIdQuery>
{
    public GetOrganizationByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}
