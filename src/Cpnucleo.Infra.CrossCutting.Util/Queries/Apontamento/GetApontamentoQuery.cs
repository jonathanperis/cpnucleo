namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento;

public class GetApontamentoQuery : BaseQuery, IRequest<ApontamentoViewModel>
{
    public Guid Id { get; set; }
}