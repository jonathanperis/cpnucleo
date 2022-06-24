using Cpnucleo.Shared.Common.DTOs;
using Cpnucleo.Shared.Common.Models;
using Cpnucleo.Shared.Queries;

namespace Cpnucleo.Shared.Queries.Impedimento;

public record ListImpedimentoViewModel : BaseQuery
{
    public IEnumerable<ImpedimentoDTO> Impedimentos { get; set; }
    public OperationResult OperationResult { get; set; }
}
