namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.Apontamento;

public class GetApontamentoQuery : BaseQuery, IRequest<GetApontamentoViewModel>
{
    public Guid Id { get; set; }
}
