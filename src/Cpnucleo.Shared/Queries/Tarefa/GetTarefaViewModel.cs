using Cpnucleo.Shared.Common.DTOs;
using Cpnucleo.Shared.Common.Models;

namespace Cpnucleo.Shared.Queries.Tarefa;

public record GetTarefaViewModel : BaseQuery
{
    public TarefaDTO Tarefa { get; set; }
    public OperationResult OperationResult { get; set; }
}
