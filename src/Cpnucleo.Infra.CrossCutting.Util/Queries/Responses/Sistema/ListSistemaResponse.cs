using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System.Collections.Generic;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Sistema
{
    public class ListSistemaResponse
    {
        public OperationResult Status { get; set; }
        public IEnumerable<SistemaViewModel> Sistemas { get; set; }
    }
}
