namespace Cpnucleo.Shared.Queries.GetProjeto;

public sealed class GetProjetoQueryValidator : AbstractValidator<GetProjetoQuery>
{
    public GetProjetoQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Necessário informar o Id do Projeto");
    }
}
