namespace Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto;

public class GetByProjetoQuery : BaseQuery, IRequest<IEnumerable<RecursoProjetoViewModel>>
{
    public Guid IdProjeto { get; set; }
}