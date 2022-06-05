namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa;

public class GetTarefaByRecursoQuery : BaseQuery, IRequest<GetTarefaByRecursoViewModel>
{
    public Guid IdRecurso { get; set; }
}
