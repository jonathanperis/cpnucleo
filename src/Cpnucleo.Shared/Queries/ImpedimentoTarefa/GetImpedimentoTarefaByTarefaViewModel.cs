using Cpnucleo.Shared.Common.DTOs;
using Cpnucleo.Shared.Common.Models;

namespace Cpnucleo.Shared.Queries.ImpedimentoTarefa;

public record GetImpedimentoTarefaByTarefaViewModel : BaseQuery
{
    public IEnumerable<ImpedimentoTarefaDTO> ImpedimentoTarefas { get; set; }
    public OperationResult OperationResult { get; set; }
}
