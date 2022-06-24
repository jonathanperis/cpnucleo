using Cpnucleo.Shared.Queries.Projeto;

namespace Cpnucleo.Application.Queries.Projeto;

public class GetProjetoQueryValidator : AbstractValidator<GetProjetoQuery>
{
    public GetProjetoQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
