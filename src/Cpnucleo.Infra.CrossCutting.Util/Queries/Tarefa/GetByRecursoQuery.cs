namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa;

public class GetByRecursoQuery : BaseQuery, IRequest<IEnumerable<TarefaViewModel>>
{
    public Guid IdRecurso { get; set; }
}