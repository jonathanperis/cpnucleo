namespace Cpnucleo.Application.Queries.RecursoTarefa.GetByTarefa;

public class GetRecursoTarefaByTarefaQuery : IRequest<GetRecursoTarefaByTarefaViewModel>
{
    public Guid IdTarefa { get; set; }
}
