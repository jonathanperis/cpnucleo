namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa;

public class GetTarefaQuery : BaseQuery, IRequest<TarefaViewModel>
{
    public Guid Id { get; set; }
}