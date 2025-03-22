namespace Application.UseCases.Project.CreateProject;

public sealed class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");

        RuleFor(x => x.OrganizationId)
            .NotEmpty().WithMessage("OrganizationId is required.");
    }
}
