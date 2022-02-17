namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento;

public class GetApontamentoQuery : IRequest<GetApontamentoViewModel>
{
    public Guid Id { get; set; }
}
