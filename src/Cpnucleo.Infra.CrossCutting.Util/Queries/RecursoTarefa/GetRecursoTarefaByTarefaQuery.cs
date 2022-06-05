namespace Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa;

public class GetRecursoTarefaByTarefaQuery : BaseQuery, IRequest<GetRecursoTarefaByTarefaViewModel>
{
    public Guid IdTarefa { get; set; }
}
