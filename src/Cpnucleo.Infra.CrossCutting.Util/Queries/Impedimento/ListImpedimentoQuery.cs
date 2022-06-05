namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Impedimento;

public class ListImpedimentoQuery : BaseQuery, IRequest<ListImpedimentoViewModel>
{
    public bool GetDependencies { get; set; }
}
