using Cpnucleo.Shared.Common.DTOs;
using Cpnucleo.Shared.Common.Models;

namespace Cpnucleo.Shared.Queries.Impedimento;

public record GetImpedimentoViewModel : BaseQuery
{
    public ImpedimentoDTO Impedimento { get; set; }
    public OperationResult OperationResult { get; set; }
}
