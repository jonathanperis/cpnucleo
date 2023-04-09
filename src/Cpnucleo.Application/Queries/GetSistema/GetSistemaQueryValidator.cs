namespace Cpnucleo.Application.Queries.GetSistema;

public sealed class GetSistemaQueryValidator : AbstractValidator<GetSistemaQuery>
{
    public GetSistemaQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
