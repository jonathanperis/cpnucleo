namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso;

public class GetRecursoQuery : BaseQuery, IRequest<GetRecursoViewModel>
{
    public Guid Id { get; set; }
}
