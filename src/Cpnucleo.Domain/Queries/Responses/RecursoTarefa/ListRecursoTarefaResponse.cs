using System.Collections.Generic;

namespace Cpnucleo.Domain.Queries.Responses.RecursoTarefa
{
    public class ListRecursoTarefaResponse
    {
        public OperationResult Status { get; set; }
        public IEnumerable<Domain.Entities.RecursoTarefa> RecursoTarefas { get; set; }
    }
}
