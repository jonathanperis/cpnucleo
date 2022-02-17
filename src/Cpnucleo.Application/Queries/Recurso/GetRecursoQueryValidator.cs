namespace Cpnucleo.Application.Queries.Recurso;

public class GetRecursoQueryValidator : AbstractValidator<GetRecursoQuery>
{
    public GetRecursoQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
