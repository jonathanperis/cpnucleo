namespace Cpnucleo.Application.Queries.Projeto;

public sealed class GetProjetoQueryValidator : AbstractValidator<GetProjetoQuery>
{
    public GetProjetoQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
