using System.Collections.Generic;

namespace Cpnucleo.Domain.Queries.Responses.RecursoProjeto
{
    public class ListRecursoProjetoResponse
    {
        public OperationResult Status { get; set; }
        public IEnumerable<Domain.Entities.RecursoProjeto> RecursoProjetos { get; set; }
    }
}
