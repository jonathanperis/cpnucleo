using Cpnucleo.Shared.Common.DTOs;
using Cpnucleo.Shared.Common.Models;

namespace Cpnucleo.Shared.Queries.Sistema;

public record GetSistemaViewModel : BaseQuery
{
    public SistemaDTO Sistema { get; set; }
    public OperationResult OperationResult { get; set; }
}
