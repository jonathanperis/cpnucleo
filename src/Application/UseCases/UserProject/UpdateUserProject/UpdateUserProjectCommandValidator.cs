namespace Application.UseCases.UserProject.UpdateUserProject;

public sealed class UpdateUserProjectCommandValidator : AbstractValidator<UpdateUserProjectCommand>
{
    public UpdateUserProjectCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.ProjectId)
            .NotEmpty().WithMessage("ProjectId is required.");
    }
}
