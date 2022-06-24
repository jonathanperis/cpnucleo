using Cpnucleo.Shared.Common.DTOs;
using Cpnucleo.Shared.Common.Models;

namespace Cpnucleo.Shared.Queries.RecursoProjeto;

public record GetRecursoProjetoViewModel : BaseQuery
{
    public RecursoProjetoDTO RecursoProjeto { get; set; }
    public OperationResult OperationResult { get; set; }
}
