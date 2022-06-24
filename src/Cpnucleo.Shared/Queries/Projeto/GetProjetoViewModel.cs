using Cpnucleo.Shared.Common.DTOs;
using Cpnucleo.Shared.Common.Models;
using Cpnucleo.Shared.Queries;

namespace Cpnucleo.Shared.Queries.Projeto;

public record GetProjetoViewModel : BaseQuery
{
    public ProjetoDTO Projeto { get; set; }
    public OperationResult OperationResult { get; set; }
}
