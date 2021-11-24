namespace Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto;

public class GetRecursoProjetoQuery : BaseQuery, IRequest<RecursoProjetoViewModel>
{
    public Guid Id { get; set; }
}