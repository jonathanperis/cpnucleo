using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System.Collections.Generic;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.RecursoProjeto
{
    public class GetByProjetoResponse
    {
        public OperationResult Status { get; set; }
        public IEnumerable<RecursoProjetoViewModel> RecursoProjetos { get; set; }
    }
}
