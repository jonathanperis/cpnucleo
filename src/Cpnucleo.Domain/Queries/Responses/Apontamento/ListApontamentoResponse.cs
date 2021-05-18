using System.Collections.Generic;

namespace Cpnucleo.Domain.Queries.Responses.Apontamento
{
    public class ListApontamentoResponse
    {
        public OperationResult Status { get; set; }
        public IEnumerable<Domain.Entities.Apontamento> Apontamentos { get; set; }
    }
}
