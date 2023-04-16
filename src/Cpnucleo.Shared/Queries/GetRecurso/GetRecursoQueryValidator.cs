namespace Cpnucleo.Shared.Queries.GetRecurso;

public sealed class GetRecursoQueryValidator : AbstractValidator<GetRecursoQuery>
{
    public GetRecursoQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Necessário informar o Id do Recurso");
    }
}
