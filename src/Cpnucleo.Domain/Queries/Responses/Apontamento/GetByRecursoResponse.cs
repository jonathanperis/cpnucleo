using System.Collections.Generic;

namespace Cpnucleo.Domain.Queries.Responses.Apontamento
{
    public class GetByRecursoResponse
    {
        public OperationResult Status { get; set; }
        public IEnumerable<Entities.Apontamento> Apontamentos { get; set; }
    }
}
