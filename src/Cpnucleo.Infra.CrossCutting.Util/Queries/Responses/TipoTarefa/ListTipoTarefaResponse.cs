using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System.Collections.Generic;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.TipoTarefa
{
    public class ListTipoTarefaResponse
    {
        public OperationResult Status { get; set; }
        public IEnumerable<TipoTarefaViewModel> TipoTarefas { get; set; }
    }
}
