namespace Cpnucleo.Application.Queries.Impedimento.ListImpedimento;

public class ListImpedimentoQuery : IRequest<ListImpedimentoViewModel>
{
    public bool GetDependencies { get; set; }
}
