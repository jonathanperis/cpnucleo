namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.Impedimento;

public class ListImpedimentoQuery : BaseQuery, IRequest<ListImpedimentoViewModel>
{
    public bool GetDependencies { get; set; }
}
