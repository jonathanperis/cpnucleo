namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Impedimento;

public class ListImpedimentoQuery : IRequest<ListImpedimentoViewModel>
{
    public bool GetDependencies { get; set; }
}
