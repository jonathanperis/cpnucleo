namespace Cpnucleo.Application.Queries.Tarefa.GetByRecurso;

public class GetByRecursoQuery : IRequest<GetByRecursoViewModel>
{
    public Guid IdRecurso { get; set; }
}
