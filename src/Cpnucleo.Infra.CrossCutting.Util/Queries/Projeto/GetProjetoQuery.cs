namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Projeto;

public class GetProjetoQuery : BaseQuery, IRequest<ProjetoViewModel>
{
    public Guid Id { get; set; }
}