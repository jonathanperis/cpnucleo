using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System.Collections.Generic;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.RecursoTarefa
{
    public class ListRecursoTarefaResponse
    {
        public OperationResult Status { get; set; }
        public IEnumerable<RecursoTarefaViewModel> RecursoTarefas { get; set; }
    }
}
