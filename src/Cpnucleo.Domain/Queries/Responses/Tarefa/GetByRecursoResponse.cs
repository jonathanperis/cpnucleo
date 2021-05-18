using System.Collections.Generic;

namespace Cpnucleo.Domain.Queries.Responses.Tarefa
{
    public class GetByRecursoResponse
    {
        public OperationResult Status { get; set; }
        public IEnumerable<Entities.Tarefa> Tarefas { get; set; }
    }
}
