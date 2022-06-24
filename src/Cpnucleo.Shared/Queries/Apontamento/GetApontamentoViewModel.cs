using Cpnucleo.Shared.Common.DTOs;
using Cpnucleo.Shared.Common.Models;
using Cpnucleo.Shared.Queries;

namespace Cpnucleo.Shared.Queries.Apontamento;

public record GetApontamentoViewModel : BaseQuery
{
    public ApontamentoDTO Apontamento { get; set; }
    public OperationResult OperationResult { get; set; }
}
