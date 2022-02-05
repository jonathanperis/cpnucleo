namespace Cpnucleo.Application.Queries.Recurso.GetRecurso;

public class GetRecursoQuery : IRequest<GetRecursoViewModel>
{
    public Guid Id { get; set; }
}
