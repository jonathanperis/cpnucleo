namespace Cpnucleo.Application.Queries.Tarefa.GetTarefa;

public class GetTarefaQuery : IRequest<GetTarefaViewModel>
{
    public Guid Id { get; set; }
}
