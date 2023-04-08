using Cpnucleo.Shared.Queries.GetProjeto;

namespace Cpnucleo.Application.Queries.GetProjeto;

public sealed class GetProjetoQueryValidator : AbstractValidator<GetProjetoQuery>
{
    public GetProjetoQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
