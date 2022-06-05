namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento;

public class GetApontamentoQuery : BaseQuery, IRequest<GetApontamentoViewModel>
{
    public Guid Id { get; set; }
}
