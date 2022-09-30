namespace Cpnucleo.Application.Queries.RecursoProjeto;

public sealed class GetRecursoProjetoQueryValidator : AbstractValidator<GetRecursoProjetoQuery>
{
    public GetRecursoProjetoQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
