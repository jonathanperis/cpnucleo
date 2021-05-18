using System.Collections.Generic;

namespace Cpnucleo.Domain.Queries.Responses.ImpedimentoTarefa
{
    public class ListImpedimentoTarefaResponse
    {
        public OperationResult Status { get; set; }
        public IEnumerable<Domain.Entities.ImpedimentoTarefa> ImpedimentoTarefas { get; set; }
    }
}
