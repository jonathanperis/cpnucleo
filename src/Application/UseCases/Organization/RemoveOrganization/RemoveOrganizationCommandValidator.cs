namespace Application.UseCases.Organization.RemoveOrganization;

public sealed class RemoveOrganizationCommandValidator : AbstractValidator<RemoveOrganizationCommand>
{
    public RemoveOrganizationCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}
