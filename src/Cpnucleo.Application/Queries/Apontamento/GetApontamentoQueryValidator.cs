using Cpnucleo.Shared.Queries.Apontamento;

namespace Cpnucleo.Application.Queries.Apontamento;

public class GetApontamentoQueryValidator : AbstractValidator<GetApontamentoQuery>
{
    public GetApontamentoQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
