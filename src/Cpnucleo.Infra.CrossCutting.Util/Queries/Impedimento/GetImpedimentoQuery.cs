namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Impedimento;

public class GetImpedimentoQuery : BaseQuery, IRequest<GetImpedimentoViewModel>
{
    public Guid Id { get; set; }
}
