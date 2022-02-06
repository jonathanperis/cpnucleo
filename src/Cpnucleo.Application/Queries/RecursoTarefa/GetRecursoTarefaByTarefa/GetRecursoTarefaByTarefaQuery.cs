namespace Cpnucleo.Application.Queries.RecursoTarefa.GetRecursoTarefaByTarefa;

public class GetRecursoTarefaByTarefaQuery : IRequest<GetRecursoTarefaByTarefaViewModel>
{
    public Guid IdTarefa { get; set; }
}
