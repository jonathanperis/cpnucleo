namespace Application.UseCases.System.RemoveSystem;

public sealed class RemoveSystemCommandValidator : AbstractValidator<RemoveSystemCommand>
{
    public RemoveSystemCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}
