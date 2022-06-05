namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Projeto;

public class GetProjetoQuery : BaseQuery, IRequest<GetProjetoViewModel>
{
    public Guid Id { get; set; }
}
