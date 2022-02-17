namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa;

public class GetTarefaByRecursoQuery : IRequest<GetTarefaByRecursoViewModel>
{
    public Guid IdRecurso { get; set; }
}
