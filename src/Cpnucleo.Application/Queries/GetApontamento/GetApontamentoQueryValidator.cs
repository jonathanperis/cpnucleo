using Cpnucleo.Shared.Queries.GetApontamento;

namespace Cpnucleo.Application.Queries.GetApontamento;

public sealed class GetApontamentoQueryValidator : AbstractValidator<GetApontamentoQuery>
{
    public GetApontamentoQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
