using Cpnucleo.Shared.Common.DTOs;
using Cpnucleo.Shared.Common.Models;
using Cpnucleo.Shared.Queries;

namespace Cpnucleo.Shared.Queries.Tarefa;

public record GetTarefaByRecursoViewModel : BaseQuery
{
    public IEnumerable<TarefaDTO> Tarefas { get; set; }
    public OperationResult OperationResult { get; set; }
}
