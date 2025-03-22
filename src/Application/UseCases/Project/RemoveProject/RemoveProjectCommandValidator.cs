namespace Application.UseCases.Project.RemoveProject;

public sealed class RemoveProjectCommandValidator : AbstractValidator<RemoveProjectCommand>
{
    public RemoveProjectCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}
