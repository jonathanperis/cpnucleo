namespace Cpnucleo.Application.Queries.Recurso;

public sealed class GetRecursoQueryValidator : AbstractValidator<GetRecursoQuery>
{
    public GetRecursoQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
