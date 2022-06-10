namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.Tarefa;

public class GetTarefaByRecursoQuery : BaseQuery, IRequest<GetTarefaByRecursoViewModel>
{
    public Guid IdRecurso { get; set; }
}
