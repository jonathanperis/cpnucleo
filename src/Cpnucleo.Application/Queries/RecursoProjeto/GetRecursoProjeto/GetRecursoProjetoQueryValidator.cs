namespace Cpnucleo.Application.Queries.RecursoProjeto.GetRecursoProjeto;

public class GetRecursoProjetoQueryValidator : AbstractValidator<GetRecursoProjetoQuery>
{
    public GetRecursoProjetoQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
