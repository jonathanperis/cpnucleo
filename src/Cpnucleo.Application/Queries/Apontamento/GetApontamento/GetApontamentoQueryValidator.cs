namespace Cpnucleo.Application.Queries.Apontamento.GetApontamento;

public class GetApontamentoQueryValidator : AbstractValidator<GetApontamentoQuery>
{
    public GetApontamentoQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
