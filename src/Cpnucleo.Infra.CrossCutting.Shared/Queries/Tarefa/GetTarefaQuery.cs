namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.Tarefa;

public class GetTarefaQuery : BaseQuery, IRequest<GetTarefaViewModel>
{
    public Guid Id { get; set; }
}
