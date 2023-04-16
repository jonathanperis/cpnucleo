namespace Cpnucleo.Shared.Queries.ListRecursoProjetoByProjeto;

public sealed class ListRecursoProjetoByProjetoQueryValidator : AbstractValidator<ListRecursoProjetoByProjetoQuery>
{
    public ListRecursoProjetoByProjetoQueryValidator()
    {
        RuleFor(x => x.IdProjeto)
            .NotEmpty()
            .WithMessage("Recurso Projeto deve conter um Projeto");
    }
}
