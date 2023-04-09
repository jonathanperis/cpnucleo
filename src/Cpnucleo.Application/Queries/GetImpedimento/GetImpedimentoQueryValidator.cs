namespace Cpnucleo.Application.Queries.GetImpedimento;

public sealed class GetImpedimentoQueryValidator : AbstractValidator<GetImpedimentoQuery>
{
    public GetImpedimentoQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
