namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.Projeto;

public class GetProjetoQuery : BaseQuery, IRequest<GetProjetoViewModel>
{
    public Guid Id { get; set; }
}
