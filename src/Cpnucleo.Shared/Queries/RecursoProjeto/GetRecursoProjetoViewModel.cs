using Cpnucleo.Shared.Common.DTOs;
using Cpnucleo.Shared.Common.Models;
using Cpnucleo.Shared.Queries;

namespace Cpnucleo.Shared.Queries.RecursoProjeto;

public record GetRecursoProjetoViewModel : BaseQuery
{
    public RecursoProjetoDTO RecursoProjeto { get; set; }
    public OperationResult OperationResult { get; set; }
}
