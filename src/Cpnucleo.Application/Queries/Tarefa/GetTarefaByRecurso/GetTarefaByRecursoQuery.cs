namespace Cpnucleo.Application.Queries.Tarefa.GetTarefaByRecurso;

public class GetTarefaByRecursoQuery : IRequest<GetTarefaByRecursoViewModel>
{
    public Guid IdRecurso { get; set; }
}
