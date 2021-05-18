using System.Collections.Generic;

namespace Cpnucleo.Domain.Queries.Responses.Recurso
{
    public class ListRecursoResponse
    {
        public OperationResult Status { get; set; }
        public IEnumerable<Domain.Entities.Recurso> Recursos { get; set; }
    }
}
