namespace Application.UseCases.Impediment.CreateImpediment;

public sealed class CreateImpedimentCommandValidator : AbstractValidator<CreateImpedimentCommand>
{
    public CreateImpedimentCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");
    }
}
