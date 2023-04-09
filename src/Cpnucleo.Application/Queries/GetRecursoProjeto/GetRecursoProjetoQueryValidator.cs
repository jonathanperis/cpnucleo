namespace Cpnucleo.Application.Queries.GetRecursoProjeto;

public sealed class GetRecursoProjetoQueryValidator : AbstractValidator<GetRecursoProjetoQuery>
{
    public GetRecursoProjetoQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
