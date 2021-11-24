namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Impedimento;

public class GetImpedimentoQuery : BaseQuery, IRequest<ImpedimentoViewModel>
{
    public Guid Id { get; set; }
}