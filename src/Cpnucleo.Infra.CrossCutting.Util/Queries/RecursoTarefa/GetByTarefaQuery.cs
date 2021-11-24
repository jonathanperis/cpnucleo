namespace Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa;

public class GetByTarefaQuery : BaseQuery, IRequest<IEnumerable<RecursoTarefaViewModel>>
{
    public Guid IdTarefa { get; set; }
}