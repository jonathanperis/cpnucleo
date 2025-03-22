namespace Application.UseCases.UserProject.CreateUserProject;

public sealed class CreateUserProjectCommandValidator : AbstractValidator<CreateUserProjectCommand>
{
    public CreateUserProjectCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.ProjectId)
            .NotEmpty().WithMessage("ProjectId is required.");
    }
}
