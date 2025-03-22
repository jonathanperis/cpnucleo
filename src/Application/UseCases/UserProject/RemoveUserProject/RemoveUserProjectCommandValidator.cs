namespace Application.UseCases.UserProject.RemoveUserProject;

public sealed class RemoveUserProjectCommandValidator : AbstractValidator<RemoveUserProjectCommand>
{
    public RemoveUserProjectCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}
