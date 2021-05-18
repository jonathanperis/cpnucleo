using System.Collections.Generic;

namespace Cpnucleo.Domain.Queries.Responses.RecursoProjeto
{
    public class GetByProjetoResponse
    {
        public OperationResult Status { get; set; }
        public IEnumerable<Entities.RecursoProjeto> RecursoProjetos { get; set; }
    }
}
