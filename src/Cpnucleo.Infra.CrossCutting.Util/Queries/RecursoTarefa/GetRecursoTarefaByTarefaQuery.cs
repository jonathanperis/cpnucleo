namespace Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa;

public class GetRecursoTarefaByTarefaQuery : IRequest<GetRecursoTarefaByTarefaViewModel>
{
    public Guid IdTarefa { get; set; }
}
