using Cpnucleo.Shared.Common.DTOs;
using Cpnucleo.Shared.Common.Models;

namespace Cpnucleo.Shared.Queries.ImpedimentoTarefa;

public record GetImpedimentoTarefaViewModel : BaseQuery
{
    public ImpedimentoTarefaDTO ImpedimentoTarefa { get; set; }
    public OperationResult OperationResult { get; set; }
}
