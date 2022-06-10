namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.Recurso;

public class GetRecursoQuery : BaseQuery, IRequest<GetRecursoViewModel>
{
    public Guid Id { get; set; }
}
