namespace Cpnucleo.Application.Queries.Recurso.GetRecurso;

public class GetRecursoQueryValidator : AbstractValidator<GetRecursoQuery>
{
    public GetRecursoQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
