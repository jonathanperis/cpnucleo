using System.Collections.Generic;

namespace Cpnucleo.Domain.Queries.Responses.Impedimento
{
    public class ListImpedimentoResponse
    {
        public OperationResult Status { get; set; }
        public IEnumerable<Domain.Entities.Impedimento> Impedimentos { get; set; }
    }
}
