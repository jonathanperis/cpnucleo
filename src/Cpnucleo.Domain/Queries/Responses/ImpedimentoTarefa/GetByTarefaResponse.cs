using System.Collections.Generic;

namespace Cpnucleo.Domain.Queries.Responses.ImpedimentoTarefa
{
    public class GetByTarefaResponse
    {
        public OperationResult Status { get; set; }
        public IEnumerable<Entities.ImpedimentoTarefa> ImpedimentoTarefas { get; set; }
    }
}
