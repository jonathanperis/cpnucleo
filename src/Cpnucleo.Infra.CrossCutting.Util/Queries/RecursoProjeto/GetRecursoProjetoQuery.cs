namespace Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto;

public class GetRecursoProjetoQuery : BaseQuery, IRequest<GetRecursoProjetoViewModel>
{
    public Guid Id { get; set; }
}
