namespace Application.UseCases.Impediment.UpdateImpediment;

public sealed class UpdateImpedimentCommandValidator : AbstractValidator<UpdateImpedimentCommand>
{
    public UpdateImpedimentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");
    }
}
