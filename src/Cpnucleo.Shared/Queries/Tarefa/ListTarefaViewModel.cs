using Cpnucleo.Shared.Common.DTOs;
using Cpnucleo.Shared.Common.Models;

namespace Cpnucleo.Shared.Queries.Tarefa;

public record ListTarefaViewModel : BaseQuery
{
    public IEnumerable<TarefaDTO> Tarefas { get; set; }
    public OperationResult OperationResult { get; set; }
}
