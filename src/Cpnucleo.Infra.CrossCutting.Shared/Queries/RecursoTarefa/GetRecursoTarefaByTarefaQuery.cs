namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.RecursoTarefa;

public class GetRecursoTarefaByTarefaQuery : BaseQuery, IRequest<GetRecursoTarefaByTarefaViewModel>
{
    public Guid IdTarefa { get; set; }
}
