namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa;

public class GetTarefaQuery : IRequest<GetTarefaViewModel>
{
    public Guid Id { get; set; }
}
