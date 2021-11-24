namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso;

public class GetRecursoQuery : BaseQuery, IRequest<RecursoViewModel>
{
    public Guid Id { get; set; }
}