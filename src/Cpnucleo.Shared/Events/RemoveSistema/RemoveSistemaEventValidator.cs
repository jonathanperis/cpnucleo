namespace Cpnucleo.Shared.Queries.GetApontamento;

public sealed class RemoveSistemaEventValidator : AbstractValidator<RemoveSistemaEvent>
{
    public RemoveSistemaEventValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Necessário informar o Id do Sistema");

        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("Necessário informar o Nome do Sistema");
    }
}
