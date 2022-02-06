namespace Cpnucleo.Application.Queries.Tarefa.GetByRecurso;

public class GetTarefaByRecursoQuery : IRequest<GetTarefaByRecursoViewModel>
{
    public Guid IdRecurso { get; set; }
}
