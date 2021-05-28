using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System.Collections.Generic;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.ImpedimentoTarefa
{
    public class GetByTarefaResponse
    {
        public OperationResult Status { get; set; }
        public IEnumerable<ImpedimentoTarefaViewModel> ImpedimentoTarefas { get; set; }
    }
}
