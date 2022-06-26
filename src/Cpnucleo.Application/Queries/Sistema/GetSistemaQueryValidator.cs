namespace Cpnucleo.Application.Queries.Sistema;

public class GetSistemaQueryValidator : AbstractValidator<GetSistemaQuery>
{
    public GetSistemaQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
