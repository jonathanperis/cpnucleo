namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Impedimento;

public class ListImpedimentoQuery : BaseQuery, IRequest<IEnumerable<ImpedimentoViewModel>>
{
    public bool GetDependencies { get; set; }
}