namespace Cpnucleo.Application.Queries.Apontamento.ListApontamento;

public class ListApontamentoQuery : IRequest<ListApontamentoViewModel>
{
    public bool GetDependencies { get; set; }
}
