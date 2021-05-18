using System.Collections.Generic;

namespace Cpnucleo.Domain.Queries.Responses.Tarefa
{
    public class ListTarefaResponse
    {
        public OperationResult Status { get; set; }
        public IEnumerable<Domain.Entities.Tarefa> Tarefas { get; set; }
    }
}
