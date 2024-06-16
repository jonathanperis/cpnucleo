namespace Application.UseCases.Project.CreateProject;

public sealed class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");

        RuleFor(x => x.SystemId)
            .NotEmpty().WithMessage("SystemId is required.");
    }
}
