namespace Cpnucleo.Shared.Queries.GetImpedimento;

public sealed class GetImpedimentoQueryValidator : AbstractValidator<GetImpedimentoQuery>
{
    public GetImpedimentoQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
