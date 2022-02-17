namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Impedimento;

public class GetImpedimentoQuery : IRequest<GetImpedimentoViewModel>
{
    public Guid Id { get; set; }
}
