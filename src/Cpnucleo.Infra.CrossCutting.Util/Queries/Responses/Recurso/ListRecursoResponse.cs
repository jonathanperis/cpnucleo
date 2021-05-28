using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System.Collections.Generic;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Recurso
{
    public class ListRecursoResponse
    {
        public OperationResult Status { get; set; }
        public IEnumerable<RecursoViewModel> Recursos { get; set; }
    }
}
