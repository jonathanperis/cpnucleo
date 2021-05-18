using System.Collections.Generic;

namespace Cpnucleo.Domain.Queries.Responses.TipoTarefa
{
    public class ListTipoTarefaResponse
    {
        public OperationResult Status { get; set; }
        public IEnumerable<Domain.Entities.TipoTarefa> TipoTarefas { get; set; }
    }
}
