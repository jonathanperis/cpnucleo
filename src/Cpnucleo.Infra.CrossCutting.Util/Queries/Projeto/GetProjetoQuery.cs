namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Projeto;

public class GetProjetoQuery : IRequest<GetProjetoViewModel>
{
    public Guid Id { get; set; }
}
