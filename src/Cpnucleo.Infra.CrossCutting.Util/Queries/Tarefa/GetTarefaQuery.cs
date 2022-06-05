namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa;

public class GetTarefaQuery : BaseQuery, IRequest<GetTarefaViewModel>
{
    public Guid Id { get; set; }
}
