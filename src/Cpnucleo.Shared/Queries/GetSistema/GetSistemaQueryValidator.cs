namespace Cpnucleo.Shared.Queries.GetSistema;

public sealed class GetSistemaQueryValidator : AbstractValidator<GetSistemaQuery>
{
    public GetSistemaQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Necessário informar o Id do Sistema");
    }
}
