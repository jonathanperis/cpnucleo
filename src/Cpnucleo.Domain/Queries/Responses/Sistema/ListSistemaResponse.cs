using System.Collections.Generic;

namespace Cpnucleo.Domain.Queries.Responses.Sistema
{
    public class ListSistemaResponse
    {
        public OperationResult Status { get; set; }
        public IEnumerable<Domain.Entities.Sistema> Sistemas { get; set; }
    }
}
