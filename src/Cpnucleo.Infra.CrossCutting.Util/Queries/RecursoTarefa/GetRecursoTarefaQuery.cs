namespace Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa;

public class GetRecursoTarefaQuery : BaseQuery, IRequest<GetRecursoTarefaViewModel>
{
    public Guid Id { get; set; }
}
