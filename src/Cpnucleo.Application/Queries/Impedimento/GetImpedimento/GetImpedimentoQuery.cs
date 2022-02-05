namespace Cpnucleo.Application.Queries.Impedimento.GetImpedimento;

public class GetImpedimentoQuery : IRequest<GetImpedimentoViewModel>
{
    public Guid Id { get; set; }
}
