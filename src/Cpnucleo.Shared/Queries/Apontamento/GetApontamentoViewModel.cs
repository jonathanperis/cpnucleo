using Cpnucleo.Shared.Common.DTOs;
using Cpnucleo.Shared.Common.Models;

namespace Cpnucleo.Shared.Queries.Apontamento;

public record GetApontamentoViewModel : BaseQuery
{
    public ApontamentoDTO Apontamento { get; set; }
    public OperationResult OperationResult { get; set; }
}
