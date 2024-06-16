namespace Application.UseCases.System.UpdateSystem;

public sealed class UpdateSystemCommandValidator : AbstractValidator<UpdateSystemCommand>
{
    public UpdateSystemCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.");
    }
}
