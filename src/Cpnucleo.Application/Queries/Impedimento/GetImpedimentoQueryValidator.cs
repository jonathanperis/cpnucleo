namespace Cpnucleo.Application.Queries.Impedimento;

public class GetImpedimentoQueryValidator : AbstractValidator<GetImpedimentoQuery>
{
    public GetImpedimentoQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
