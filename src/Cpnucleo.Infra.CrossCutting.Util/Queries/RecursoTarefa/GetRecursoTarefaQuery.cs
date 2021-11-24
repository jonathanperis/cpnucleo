namespace Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa;

public class GetRecursoTarefaQuery : BaseQuery, IRequest<RecursoTarefaViewModel>
{
    public Guid Id { get; set; }
}