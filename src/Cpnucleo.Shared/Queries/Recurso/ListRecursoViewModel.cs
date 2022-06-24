using Cpnucleo.Shared.Common.DTOs;
using Cpnucleo.Shared.Common.Models;

namespace Cpnucleo.Shared.Queries.Recurso;

public record ListRecursoViewModel : BaseQuery
{
    public IEnumerable<RecursoDTO> Recursos { get; set; }
    public OperationResult OperationResult { get; set; }
}
