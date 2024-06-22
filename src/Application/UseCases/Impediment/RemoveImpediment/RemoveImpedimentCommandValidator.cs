namespace Application.UseCases.Impediment.RemoveImpediment;

public sealed class RemoveImpedimentCommandValidator : AbstractValidator<RemoveImpedimentCommand>
{
    public RemoveImpedimentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}
