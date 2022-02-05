namespace Cpnucleo.Application.Queries.Apontamento.GetByRecurso;

public class GetByRecursoQuery : IRequest<GetByRecursoViewModel>
{
    public Guid IdRecurso { get; set; }
}
