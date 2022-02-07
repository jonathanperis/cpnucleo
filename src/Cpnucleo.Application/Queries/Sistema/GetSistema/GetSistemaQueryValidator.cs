namespace Cpnucleo.Application.Queries.Sistema.GetSistema;

public class GetSistemaQueryValidator : AbstractValidator<GetSistemaQuery>
{
    public GetSistemaQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
