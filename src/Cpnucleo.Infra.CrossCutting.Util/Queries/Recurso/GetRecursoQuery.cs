namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso;

public class GetRecursoQuery : IRequest<GetRecursoViewModel>
{
    public Guid Id { get; set; }
}
