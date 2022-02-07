namespace Cpnucleo.Application.Queries.Projeto.GetProjeto;

public class GetProjetoQueryValidator : AbstractValidator<GetProjetoQuery>
{
    public GetProjetoQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
