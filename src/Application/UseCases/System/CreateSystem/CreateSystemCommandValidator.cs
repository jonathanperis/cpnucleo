namespace Application.UseCases.System.CreateSystem;

public sealed class CreateSystemCommandValidator : AbstractValidator<CreateSystemCommand>
{
    public CreateSystemCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.");
    }
}
