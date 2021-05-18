using System.Collections.Generic;

namespace Cpnucleo.Domain.Queries.Responses.RecursoTarefa
{
    public class GetByTarefaResponse
    {
        public OperationResult Status { get; set; }
        public IEnumerable<Entities.RecursoTarefa> RecursoTarefas { get; set; }
    }
}
