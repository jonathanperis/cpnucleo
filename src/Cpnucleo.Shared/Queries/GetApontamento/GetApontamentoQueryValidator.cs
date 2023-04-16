namespace Cpnucleo.Shared.Queries.GetApontamento;

public sealed class GetApontamentoQueryValidator : AbstractValidator<GetApontamentoQuery>
{
    public GetApontamentoQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
