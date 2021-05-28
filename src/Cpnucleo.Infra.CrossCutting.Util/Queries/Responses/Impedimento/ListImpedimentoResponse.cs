using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System.Collections.Generic;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Impedimento
{
    public class ListImpedimentoResponse
    {
        public OperationResult Status { get; set; }
        public IEnumerable<ImpedimentoViewModel> Impedimentos { get; set; }
    }
}
