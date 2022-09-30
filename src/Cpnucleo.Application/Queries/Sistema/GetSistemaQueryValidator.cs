namespace Cpnucleo.Application.Queries.Sistema;

public sealed class GetSistemaQueryValidator : AbstractValidator<GetSistemaQuery>
{
    public GetSistemaQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
