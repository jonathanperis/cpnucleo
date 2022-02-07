namespace Cpnucleo.Application.Queries.Impedimento.GetImpedimento;

public class GetImpedimentoQueryValidator : AbstractValidator<GetImpedimentoQuery>
{
    public GetImpedimentoQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
