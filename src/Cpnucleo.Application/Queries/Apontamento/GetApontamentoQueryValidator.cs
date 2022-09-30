namespace Cpnucleo.Application.Queries.Apontamento;

public sealed class GetApontamentoQueryValidator : AbstractValidator<GetApontamentoQuery>
{
    public GetApontamentoQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
